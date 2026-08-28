public class UI_StageClear : UI_Popup
{
	private enum Buttons
	{
		BackToTitleButton,
	}

	public override bool Init()
	{
		if (base.Init() == false)
			return false;

		BindButtons(typeof(Buttons));
		GetButton((int)Buttons.BackToTitleButton).gameObject.BindEvent(_ => Managers.Scene.LoadScene(Define.EScene.TitleScene));

		return true;
	}
}
