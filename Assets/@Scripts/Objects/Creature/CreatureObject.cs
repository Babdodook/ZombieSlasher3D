using System;
using UnityEngine;
using static Define;

public class CreatureObject : BaseObject
{
	public ECharacterType CreatureType { get; protected set; } = ECharacterType.None;
	public ECharacterState State { get; protected set; } = ECharacterState.Idle;

	[SerializeField] protected float _maxHp = 10f;
	public float MaxHp { get; protected set; }
	public float CurrentHp { get; protected set; }
	public bool IsDead { get; protected set; }

	public event Action<CreatureObject, float, float> OnHpChanged;
	public event Action<CreatureObject> OnDeath;

	public override bool Init()
	{
		if (base.Init() == false)
			return false;

		ObjectType = EObjectType.Character;
		MaxHp = _maxHp;
		CurrentHp = _maxHp;
		IsDead = false;
		State = ECharacterState.Idle;

		return true;
	}

	// Pooled objects are reused via SetActive(true) instead of a fresh Awake(), so
	// death/HP state left over from the previous life must be reset here every time.
	protected virtual void OnEnable()
	{
		if (_init == false)
			return;

		MaxHp = _maxHp;
		CurrentHp = _maxHp;
		IsDead = false;
		State = ECharacterState.Idle;
	}

	public virtual void TakeDamage(float damage, CreatureObject attacker = null)
	{
		if (IsDead)
			return;

		CurrentHp = Mathf.Max(0f, CurrentHp - damage);
		OnHpChanged?.Invoke(this, CurrentHp, MaxHp);

		if (CurrentHp <= 0f)
			Die();
	}

	protected virtual void Die()
	{
		if (IsDead)
			return;

		IsDead = true;
		State = ECharacterState.Dead;
		OnDeath?.Invoke(this);
	}

	protected void SetState(ECharacterState state)
	{
		State = state;
	}
}
