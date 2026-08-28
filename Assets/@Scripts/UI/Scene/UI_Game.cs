using UnityEngine;

public class UI_Game : UI_Scene
{
	private enum GameObjects
	{
		BossHpBarRoot,
	}

	private enum Images
	{
		BossGaugeFill,
		BossHpFill,
		PlayerHpFill,
		LevelXpFill,
	}

	private enum Texts
	{
		KillCountText,
		LevelText,
	}

	private PlayerController _player;

	public override bool Init()
	{
		if (base.Init() == false)
			return false;

		BindObjects(typeof(GameObjects));
		BindImages(typeof(Images));
		BindTexts(typeof(Texts));

		GetObject((int)GameObjects.BossHpBarRoot).SetActive(false);

		Managers.Stage.OnBossGaugeChanged += RefreshBossGauge;
		Managers.Stage.OnBossSpawned += OnBossSpawned;
		Managers.Level.OnXpChanged += RefreshLevelXp;

		return true;
	}

	public void BindPlayer(PlayerController player)
	{
		_player = player;
		_player.OnHpChanged += RefreshPlayerHp;
		RefreshPlayerHp(_player, _player.CurrentHp, _player.MaxHp);
	}

	private void RefreshBossGauge(int current, int threshold)
	{
		GetImage((int)Images.BossGaugeFill).fillAmount = threshold <= 0 ? 0f : (float)current / threshold;
	}

	private void OnBossSpawned(BossController boss)
	{
		GetObject((int)GameObjects.BossHpBarRoot).SetActive(true);
		boss.OnHpChanged += RefreshBossHp;
	}

	private void RefreshBossHp(CreatureObject boss, float current, float max)
	{
		GetImage((int)Images.BossHpFill).fillAmount = max <= 0f ? 0f : current / max;
	}

	private void RefreshPlayerHp(CreatureObject player, float current, float max)
	{
		GetImage((int)Images.PlayerHpFill).fillAmount = max <= 0f ? 0f : current / max;
	}

	private void RefreshLevelXp(int level, float currentXp, float xpToNext)
	{
		GetImage((int)Images.LevelXpFill).fillAmount = xpToNext <= 0f ? 0f : currentXp / xpToNext;
		GetText((int)Texts.LevelText).text = $"Lv.{level}";
	}

	private void Update()
	{
		GetText((int)Texts.KillCountText).text = Managers.Game.KillCount.ToString();
	}
}
