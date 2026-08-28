using UnityEngine;

public class UI_StageSelect : UI_Scene
{
	private enum Buttons
	{
		Stage1Button,
		Stage2Button,
		Stage3Button,
	}

	[SerializeField] private StageTableData _stageTable;

	public override bool Init()
	{
		if (base.Init() == false)
			return false;

		BindButtons(typeof(Buttons));

		for (int i = 0; i < System.Enum.GetValues(typeof(Buttons)).Length; i++)
		{
			int stageIndex = i;
			GetButton(i).gameObject.BindEvent(_ => OnClickStageButton(stageIndex));
		}

		return true;
	}

	private void OnClickStageButton(int stageIndex)
	{
		if (_stageTable == null || stageIndex >= _stageTable.Stages.Count)
			return;

		StageData stage = _stageTable.Stages[stageIndex];
		if (stage == null)
			return;

		Managers.Game.SetSelectedStage(stage);
		Managers.Scene.LoadScene(Define.EScene.GameScene);
	}
}
