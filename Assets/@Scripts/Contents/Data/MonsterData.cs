using UnityEngine;

[CreateAssetMenu(menuName = "ZombieSlasher/Monster Data", fileName = "Monster_")]
public class MonsterData : ScriptableObject
{
	public string DisplayName;
	public string DisplayNameEN;

	[Header("Base Stats")]
	public float MaxHp = 10f;
	public float MoveSpeed = 3.5f;
	public float ContactDamage = 5f;
	public float ContactDamageCooldown = 1f;
	public float XpReward = 1f;

	[Header("Stage Growth")]
	public float HpGrowthPerStage = 0f;
	public float DamageGrowthPerStage = 0f;
}
