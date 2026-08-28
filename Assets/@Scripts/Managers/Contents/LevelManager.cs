using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelManager
{
	public Transform Player { get; private set; }
	public PlayerRunStats Stats { get; private set; }

	public int CurrentLevel { get; private set; } = 1;
	public float CurrentXp { get; private set; }
	public float XpToNextLevel => _stageData == null ? 0f : _stageData.BaseXpToNextLevel + (CurrentLevel - 1) * _stageData.XpToNextLevelGrowth;

	public event Action<int, float, float> OnXpChanged;
	public event Action<int> OnLevelUp;

	private StageData _stageData;

	public void Init(StageData stageData, Transform player)
	{
		_stageData = stageData;
		Player = player;
		Stats = player.GetComponent<PlayerRunStats>();
		CurrentLevel = 1;
		CurrentXp = 0f;
	}

	public void AddXp(float amount)
	{
		if (_stageData == null)
			return;

		CurrentXp += amount;

		while (CurrentXp >= XpToNextLevel)
		{
			CurrentXp -= XpToNextLevel;
			CurrentLevel++;
			OnLevelUp?.Invoke(CurrentLevel);
		}

		OnXpChanged?.Invoke(CurrentLevel, CurrentXp, XpToNextLevel);
	}

	public List<SkillData> RollChoices(int count = 3)
	{
		if (_stageData == null || Stats == null)
			return new List<SkillData>();

		List<SkillData> available = _stageData.SkillRoster
			.Where(skill => skill != null && Stats.GetSkillLevel(skill) < skill.MaxLevel)
			.ToList();

		available.Shuffle();

		return available.Take(count).ToList();
	}

	public void ApplySkillChoice(SkillData skill)
	{
		Stats?.ApplySkill(skill);
	}

	public void Clear()
	{
		_stageData = null;
		Player = null;
		Stats = null;
		CurrentLevel = 1;
		CurrentXp = 0f;
		OnXpChanged = null;
		OnLevelUp = null;
	}
}
