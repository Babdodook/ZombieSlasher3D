using UnityEngine;
using static Define;

public class GameScene : BaseScene
{
	[SerializeField] private StageData _stageData;
	[SerializeField] private Transform _playerSpawnPoint;
	[SerializeField] private CameraFollow _cameraFollow;

	public override bool Init()
	{
		if (base.Init() == false)
			return false;

		SceneType = EScene.GameScene;

		Managers.Init();
		Managers.Resource.LoadAllAsync<GameObject>("Stage1", (key, cur, total) =>
		{
			if (cur == total)
				StartGame();
		});

		return true;
	}

	private void StartGame()
	{
		Managers.Game.Init();

		UI_Game gameUI = Managers.UI.ShowSceneUI<UI_Game>();

		GameObject playerGo = Managers.Resource.Instantiate("Player");
		playerGo.transform.position = _playerSpawnPoint.position;

		PlayerController player = playerGo.GetComponent<PlayerController>();
		player.OnDeath += _ => Managers.UI.ShowPopupUI<UI_GameOver>();

		gameUI.BindPlayer(player);

		if (_cameraFollow != null)
			_cameraFollow.SetTarget(playerGo.transform);

		Managers.Stage.Init(_stageData, playerGo.transform);
		Managers.Stage.OnStageClear += () => Managers.UI.ShowPopupUI<UI_StageClear>();
	}

	private void Update()
	{
		Managers.Game.OnUpdate();
		Managers.Stage.OnUpdate(Time.deltaTime);
	}

	public override void Clear()
	{
		Managers.Game.Clear();
		Managers.Stage.Clear();
	}
}
