using UnityEngine;
using static Define;

[CreateAssetMenu(menuName = "ZombieSlasher/Skill Data", fileName = "Skill_")]
public class SkillData : ScriptableObject
{
	public string DisplayName;
	[TextArea] public string Description;
	public string DisplayNameEN;
	[TextArea] public string DescriptionEN;
	public Sprite Icon;
	public int MaxLevel = 5;
	public float MagnitudePerLevel = 1f;
	public ESkillEffectType EffectType;

	public string GetDisplayName()
	{
		if (Managers.Localization.CurrentLanguage == ELanguage.English && string.IsNullOrEmpty(DisplayNameEN) == false)
			return DisplayNameEN;

		return DisplayName;
	}

	public string GetDescription()
	{
		if (Managers.Localization.CurrentLanguage == ELanguage.English && string.IsNullOrEmpty(DescriptionEN) == false)
			return DescriptionEN;

		return Description;
	}
}
