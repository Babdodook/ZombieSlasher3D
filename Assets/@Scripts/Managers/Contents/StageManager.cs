using System;
using UnityEngine;
using static Define;

public class StageManager
{
	public StageData CurrentStage { get; private set; }
	public int BossGaugeKillCount { get; private set; }
	public float BossGaugeRatio => CurrentStage == null ? 0f : Mathf.Clamp01(BossGaugeKillCount / (float)CurrentStage.BossGaugeKillThreshold);
	public bool BossSpawned { get; private set; }
	public BossController ActiveBoss { get; private set; }

	public event Action<int, int> OnBossGaugeChanged;
	public event Action<BossController> OnBossSpawned;
	public event Action OnStageClear;

	private MinionSpawner _spawner = new MinionSpawner();
	private Transform _player;

	public void Init(StageData stageData, Transform player)
	{
		CurrentStage = stageData;
		_player = player;
		BossGaugeKillCount = 0;
		BossSpawned = false;
		ActiveBoss = null;

		_spawner.Init(stageData);
	}

	public void OnUpdate(float dt)
	{
		if (CurrentStage == null)
			return;

		_spawner.OnUpdate(dt, _player);
	}

	public void NotifyMinionKilled()
	{
		if (CurrentStage == null || BossSpawned)
			return;

		BossGaugeKillCount++;
		OnBossGaugeChanged?.Invoke(BossGaugeKillCount, CurrentStage.BossGaugeKillThreshold);

		if (BossGaugeKillCount >= CurrentStage.BossGaugeKillThreshold)
			SpawnBoss();
	}

	private void SpawnBoss()
	{
		BossSpawned = true;
		_spawner.StopSpawning();

		GameObject go = Managers.Resource.Instantiate(CurrentStage.BossPrefabKey, pooling: true);
		if (go == null)
			return;

		Vector3 desiredPos = _player.position + _player.forward * 10f;
		Vector3 spawnPos = desiredPos;

		if (UnityEngine.AI.NavMesh.SamplePosition(desiredPos, out UnityEngine.AI.NavMeshHit navHit, 10f, UnityEngine.AI.NavMesh.AllAreas))
			spawnPos = navHit.position;
		else if (UnityEngine.AI.NavMesh.SamplePosition(_player.position, out navHit, 50f, UnityEngine.AI.NavMesh.AllAreas))
			spawnPos = navHit.position; // Desired spot is off the baked navmesh; fall back to the nearest mesh point to the player.

		UnityEngine.AI.NavMeshAgent agent = go.GetComponent<UnityEngine.AI.NavMeshAgent>();
		if (agent != null)
			agent.Warp(spawnPos);
		else
			go.transform.position = spawnPos;

		BossController boss = go.GetComponent<BossController>();
		if (boss == null)
			return;

		boss.SetTarget(_player);
		boss.OnDeath += OnBossDeath;

		ActiveBoss = boss;
		OnBossSpawned?.Invoke(boss);
	}

	private void OnBossDeath(CreatureObject boss)
	{
		Managers.Game.SetState(EGameState.StageClear);
		OnStageClear?.Invoke();
	}

	public void Clear()
	{
		CurrentStage = null;
		_player = null;
		BossGaugeKillCount = 0;
		BossSpawned = false;
		ActiveBoss = null;
		OnBossGaugeChanged = null;
		OnBossSpawned = null;
		OnStageClear = null;
	}
}
