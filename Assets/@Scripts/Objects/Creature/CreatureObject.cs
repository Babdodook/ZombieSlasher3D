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

	// Lets subclasses (e.g. MonsterController) source max HP from a data asset
	// instead of the inspector field, without CreatureObject needing to know about it.
	protected virtual float GetBaseMaxHp() => _maxHp;

	public override bool Init()
	{
		if (base.Init() == false)
			return false;

		ObjectType = EObjectType.Character;
		MaxHp = GetBaseMaxHp();
		CurrentHp = MaxHp;
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

		MaxHp = GetBaseMaxHp();
		CurrentHp = MaxHp;
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

	// Only CreatureObject can invoke OnHpChanged, so subclasses that grant max-HP
	// bonuses (e.g. a level-up skill) route through this instead of touching HP fields directly.
	protected void ModifyMaxHp(float delta, bool healByDelta)
	{
		MaxHp = Mathf.Max(1f, MaxHp + delta);
		CurrentHp = healByDelta ? Mathf.Min(MaxHp, CurrentHp + delta) : Mathf.Min(CurrentHp, MaxHp);
		OnHpChanged?.Invoke(this, CurrentHp, MaxHp);
	}
}
