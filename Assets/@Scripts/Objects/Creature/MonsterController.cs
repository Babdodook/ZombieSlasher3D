using UnityEngine;
using UnityEngine.AI;
using static Define;

[RequireComponent(typeof(NavMeshAgent))]
public class MonsterController : CreatureObject
{
	[SerializeField] protected float _moveSpeed = 3.5f;
	[SerializeField] protected float _contactDamage = 5f;
	[SerializeField] protected float _contactDamageCooldown = 1f;

	protected NavMeshAgent _agent;
	protected Transform _target;

	private float _contactTimer;

	public override bool Init()
	{
		if (base.Init() == false)
			return false;

		CreatureType = ECharacterType.Monster;

		_agent = GetComponent<NavMeshAgent>();
		_agent.speed = _moveSpeed;

		return true;
	}

	public void SetTarget(Transform target)
	{
		_target = target;
	}

	public void ApplyDifficultyScale(float hpMultiplier, float damageMultiplier)
	{
		MaxHp *= hpMultiplier;
		CurrentHp = MaxHp;
		_contactDamage *= damageMultiplier;
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
		Managers.Resource.Destroy(gameObject);
	}
}
