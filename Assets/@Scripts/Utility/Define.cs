using UnityEngine;

public static class Define
{
	public enum EScene
	{
		Unknown,
		TitleScene,
		GameScene,
	}

	public enum EUIEvent
	{
		Click,
		PointerDown,
		PointerUp,
		Drag,
	}

	public enum ESound
	{
		Bgm,
		Effect,
		Max,
	}

	public enum EGameState
	{
		Ready,
		Playing,
		Paused,
		StageClear,
		GameOver,
	}

	public enum EObjectType
	{
		None,
		Character,
		Projectile,
		Pickup,
	}

	public enum ECharacterType
	{
		None,
		Player,
		Monster,
		Boss,
	}

	public enum ECharacterState
	{
		None,
		Idle,
		Move,
		Attack,
		Dead,
	}

	public enum EBossPatternState
	{
		Chase,
		Telegraph,
		Execute,
		Recover,
	}

	public enum EEquipSlot
	{
		Shadow,
		Body,
		Shoes,
		Legs,
		Chest,
		Weapon,
		Bag,
		Shield,
		Head,
	}

	public enum EWeaponType
	{
		None,
		Melee,
		Ranged,
	}

	public enum ESkillEffectType
	{
		Damage,
		FireRate,
		MoveSpeed,
		MaxHealth,
		PickupRadius,
	}

	public enum ELanguage
	{
		Korean,
		English,
	}

	public enum ELayer
	{
		Default = 0,
		TransparentFX = 1,
		IgnoreRaycast = 2,
		Water = 4,
		UI = 5,
		Player = 6,
		Monster = 7,
		Obstacle = 8,
		Projectile = 9,
	}
}

public static class AnimName
{
	public const string IDLE = "idle";
	public const string MOVE = "move";
	public const string ATTACK = "attack";
	public const string DAMAGED = "hit";
	public const string DEAD = "dead";
}
