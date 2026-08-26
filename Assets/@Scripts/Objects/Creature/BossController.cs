using System;
using UnityEngine;
using static Define;

public class BossController : MonsterController
{
	[Serializable]
	private class GroundAoEPatternConfig
	{
		public string TelegraphPrefabKey = "AoE_CircleTelegraph";
		public float Radius = 3f;
		public float TelegraphDuration = 1.5f;
		public float Damage = 20f;
		public float RecoverTime = 1f;
	}

	[Header("Boss Pattern")]
	[SerializeField] private float _patternTriggerRange = 10f;
	[SerializeField] private float _patternCooldown = 4f;
	[SerializeField] private GroundAoEPatternConfig[] _patterns;

	private EBossPatternState _patternState = EBossPatternState.Chase;
	private float _patternTimer;

	public override bool Init()
	{
		if (base.Init() == false)
			return false;

		CreatureType = ECharacterType.Boss;
		_patternState = EBossPatternState.Chase;
		_patternTimer = 0f;

		return true;
	}

	protected override void UpdateAI()
	{
		if (_target == null)
			return;

		switch (_patternState)
		{
			case EBossPatternState.Chase:
				base.UpdateAI();

				if (_patternTimer > 0f)
					_patternTimer -= Time.deltaTime;

				if (_patternTimer <= 0f && _patterns != null && _patterns.Length > 0
					&& Vector3.Distance(transform.position, _target.position) <= _patternTriggerRange)
				{
					BeginTelegraph(_patterns[UnityEngine.Random.Range(0, _patterns.Length)]);
				}
				break;

			case EBossPatternState.Telegraph:
				break;

			case EBossPatternState.Recover:
				_patternTimer -= Time.deltaTime;
				if (_patternTimer <= 0f)
					_patternState = EBossPatternState.Chase;
				break;
		}
	}

	private void BeginTelegraph(GroundAoEPatternConfig cfg)
	{
		_agent.isStopped = true;
		SetState(ECharacterState.Attack);
		_patternState = EBossPatternState.Telegraph;

		Vector3 targetPos = _target.position;

		GameObject go = Managers.Resource.Instantiate(cfg.TelegraphPrefabKey, pooling: true);
		if (go == null)
		{
			_patternState = EBossPatternState.Chase;
			return;
		}

		AoETelegraphCircle telegraph = go.GetComponent<AoETelegraphCircle>();
		telegraph.Activate(targetPos, cfg.Radius, cfg.TelegraphDuration, cfg.Damage, () => OnTelegraphResolved(cfg));
	}

	private void OnTelegraphResolved(GroundAoEPatternConfig cfg)
	{
		if (IsDead)
			return;

		_agent.isStopped = false;
		SetState(ECharacterState.Move);
		_patternState = EBossPatternState.Recover;
		_patternTimer = cfg.RecoverTime;
	}
}
