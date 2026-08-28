using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ZombieSlasher/Stage Data", fileName = "Stage_")]
public class StageData : ScriptableObject
{
	public string StageName;

	[Header("Boss")]
	public string BossPrefabKey;
	public int BossGaugeKillThreshold = 40;

	[Header("Minion Spawn")]
	public float MinionSpawnRadiusMin = 8f;
	public float MinionSpawnRadiusMax = 12f;
	public List<WaveData> Waves = new List<WaveData>();

	[Header("Difficulty Scaling")]
	public int StageIndex = 0;
	public float DifficultyHpMultiplier = 1f;
	public float DifficultyDamageMultiplier = 1f;

	[Header("Leveling")]
	public float BaseXpToNextLevel = 10f;
	public float XpToNextLevelGrowth = 5f;
	public List<SkillData> SkillRoster = new List<SkillData>();
}
