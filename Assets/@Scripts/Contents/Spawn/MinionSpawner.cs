using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MinionSpawner
{
	private class WaveRuntime
	{
		public WaveData Data;
		public float Timer;
		public int SpawnedCount;
	}

	private StageData _stage;
	private List<WaveRuntime> _waves = new List<WaveRuntime>();
	private float _stageElapsed;
	private bool _stopped;

	public void Init(StageData stage)
	{
		_stage = stage;
		_stageElapsed = 0f;
		_stopped = false;

		_waves.Clear();
		foreach (WaveData data in stage.Waves)
			_waves.Add(new WaveRuntime { Data = data, Timer = 0f, SpawnedCount = 0 });
	}

	public void StopSpawning()
	{
		_stopped = true;
	}

	public void OnUpdate(float dt, Transform player)
	{
		if (_stopped || player == null)
			return;

		_stageElapsed += dt;

		foreach (WaveRuntime wave in _waves)
			TickWave(wave, dt, player);
	}

	private void TickWave(WaveRuntime wave, float dt, Transform player)
	{
		if (_stageElapsed < wave.Data.StartTime)
			return;

		if (wave.Data.SpawnCount >= 0 && wave.SpawnedCount >= wave.Data.SpawnCount)
			return;

		wave.Timer -= dt;
		if (wave.Timer > 0f)
			return;

		wave.Timer = wave.Data.SpawnInterval;
		SpawnOne(wave, player);
	}

	private void SpawnOne(WaveRuntime wave, Transform player)
	{
		Vector3 pos = RandomRingPoint(player.position, _stage.MinionSpawnRadiusMin, _stage.MinionSpawnRadiusMax);

		if (NavMesh.SamplePosition(pos, out NavMeshHit hit, 5f, NavMesh.AllAreas))
			pos = hit.position;
		else if (NavMesh.SamplePosition(player.position, out hit, 50f, NavMesh.AllAreas))
			pos = hit.position; // Ring point is off the baked navmesh; fall back to the nearest mesh point to the player.

		GameObject go = Managers.Resource.Instantiate(wave.Data.MonsterPrefabKey, pooling: true);
		if (go == null)
			return;

		NavMeshAgent agent = go.GetComponent<NavMeshAgent>();
		if (agent != null)
			agent.Warp(pos);
		else
			go.transform.position = pos;

		MonsterController monster = go.GetComponent<MonsterController>();
		if (monster != null)
		{
			monster.SetTarget(player);
			monster.ApplyDifficultyScale(_stage.DifficultyHpMultiplier, _stage.DifficultyDamageMultiplier);
		}

		wave.SpawnedCount++;
	}

	private Vector3 RandomRingPoint(Vector3 center, float minRadius, float maxRadius)
	{
		Vector2 dir = Random.insideUnitCircle.normalized;
		float radius = Random.Range(minRadius, maxRadius);
		return center + new Vector3(dir.x, 0f, dir.y) * radius;
	}
}
