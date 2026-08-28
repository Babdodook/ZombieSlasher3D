using UnityEngine;

public class TitleScene : BaseScene
{
	public override bool Init()
	{
		if (base.Init() == false)
			return false;

		SceneType = Define.EScene.TitleScene;
		Managers.Init();

		Managers.Resource.LoadAllAsync<GameObject>("Title", (key, cur, total) =>
		{
			if (cur == total)
				Managers.UI.ShowSceneUI<UI_StageSelect>();
		});

		return true;
	}

	public override void Clear()
	{
	}
}
