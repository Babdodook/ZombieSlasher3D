using UnityEngine;
using static Define;

public class GameScene : BaseScene
{
	public override bool Init()
	{
		if (base.Init() == false)
			return false;

		SceneType = EScene.GameScene;

		Managers.Init();
		Managers.Game.Init();

		return true;
	}

	private void Update()
	{
		Managers.Game.OnUpdate();
	}

	public override void Clear()
	{
		Managers.Game.Clear();
	}
}
