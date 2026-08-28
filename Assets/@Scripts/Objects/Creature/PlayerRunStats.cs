using System.Collections.Generic;
using UnityEngine;
using static Define;

public class PlayerRunStats : MonoBehaviour
{
	[SerializeField] private float _basePickupRadius = 3f;

	public float DamageMultiplier { get; private set; } = 1f;
	public float FireRateMultiplier { get; private set; } = 1f;
	public float MoveSpeedMultiplier { get; private set; } = 1f;
	public float PickupRadiusMultiplier { get; private set; } = 1f;
	public float PickupRadius => _basePickupRadius * PickupRadiusMultiplier;

	private readonly Dictionary<SkillData, int> _skillLevels = new Dictionary<SkillData, int>();

	private PlayerController _player;

	private void Awake()
	{
		_player = GetComponent<PlayerController>();
	}

	public int GetSkillLevel(SkillData skill)
	{
		return _skillLevels.TryGetValue(skill, out int level) ? level : 0;
	}

	public void ApplySkill(SkillData skill)
	{
		_skillLevels[skill] = GetSkillLevel(skill) + 1;

		switch (skill.EffectType)
		{
			case ESkillEffectType.Damage:
				DamageMultiplier += skill.MagnitudePerLevel;
				break;
			case ESkillEffectType.FireRate:
				FireRateMultiplier += skill.MagnitudePerLevel;
				break;
			case ESkillEffectType.MoveSpeed:
				MoveSpeedMultiplier += skill.MagnitudePerLevel;
				break;
			case ESkillEffectType.PickupRadius:
				PickupRadiusMultiplier += skill.MagnitudePerLevel;
				break;
			case ESkillEffectType.MaxHealth:
				_player.ApplyMaxHpBonus(skill.MagnitudePerLevel);
				break;
		}
	}
}
