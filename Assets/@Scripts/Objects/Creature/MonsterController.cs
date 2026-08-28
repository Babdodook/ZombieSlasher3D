using UnityEngine;
using UnityEngine.AI;
using static Define;

[RequireComponent(typeof(NavMeshAgent))]
public class MonsterController : CreatureObject
{
	[SerializeField] protected MonsterData _data;

	protected NavMeshAgent _agent;
	protected Transform _target;

	private float _contactDamage;
	private float _contactDamageCooldown;
	private float _contactTimer;

	protected override float GetBaseMaxHp() => _data != null ? _data.MaxHp : base.GetBaseMaxHp();

	public override bool Init()
	{
		if (base.Init() == false)
			return false;

		CreatureType = ECharacterType.Monster;

		_agent = GetComponent<NavMeshAgent>();
		_agent.speed = _data != null ? _data.MoveSpeed : _agent.speed;
		_contactDamage = _data != null ? _data.ContactDamage : 0f;
		_contactDamageCooldown = _data != null ? _data.ContactDamageCooldown : 1f;

		return true;
	}

	protected override void OnEnable()
	{
		base.OnEnable();
		_contactTimer = 0f;
		_contactDamage = _data != null ? _data.ContactDamage : 0f;
		_contactDamageCooldown = _data != null ? _data.ContactDamageCooldown : 1f;
	}

	public void SetTarget(Transform target)
	{
		_target = target;
	}

	// stageIndex feeds MonsterData's per-stage growth so the same monster can be
	// reused across stages while still getting progressively stronger.
	public void ApplyDifficultyScale(int stageIndex, float hpMultiplier, float damageMultiplier)
	{
		float hpGrowth = _data != null ? _data.HpGrowthPerStage * stageIndex : 0f;
		float damageGrowth = _data != null ? _data.DamageGrowthPerStage * stageIndex : 0f;

		MaxHp = (GetBaseMaxHp() + hpGrowth) * hpMultiplier;
		CurrentHp = MaxHp;
		_contactDamage = (_contactDamage + damageGrowth) * damageMultiplier;
	}

	protected virtual void Update()
	{
		if (IsDead)
			return;

		UpdateAI();

		if (_contactTimer > 0f)
			_contactTimer -= Time.deltaTime;
	}

	protected virtual void UpdateAI()
	{
		if (_target == null)
			return;

		_agent.isStopped = false;
		_agent.SetDestination(_target.position);
		SetState(ECharacterState.Move);
	}

	private void OnTriggerStay(Collider other)
	{
		TryContactDamage(other);
	}

	private void OnTriggerEnter(Collider other)
	{
		TryContactDamage(other);
	}

	private void TryContactDamage(Collider other)
	{
		if (IsDead || _contactTimer > 0f)
			return;

		if (other.gameObject.layer != (int)ELayer.Player)
			return;

		CreatureObject player = other.GetComponentInParent<CreatureObject>();
		if (player == null)
			return;

		player.TakeDamage(_contactDamage, this);
		_contactTimer = _contactDamageCooldown;
	}

	protected override void Die()
	{
		base.Die();

		Managers.Game.AddKill();
		Managers.Stage.NotifyMinionKilled();
		SpawnExperienceGem();
		Managers.Resource.Destroy(gameObject);
	}

	private void SpawnExperienceGem()
	{
		GameObject go = Managers.Resource.Instantiate("ExperienceGem", pooling: true);
		if (go == null)
			return;

		go.GetComponent<ExperienceGem>()?.Activate(transform.position, _data != null ? _data.XpReward : 0f);
	}
}
