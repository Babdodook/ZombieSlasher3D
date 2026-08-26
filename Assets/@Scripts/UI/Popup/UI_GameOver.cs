public class UI_GameOver : UI_Popup
{
	private enum Buttons
	{
		RestartButton,
	}

	public override bool Init()
	{
		if (base.Init() == false)
			return false;

		BindButtons(typeof(Buttons));
		GetButton((int)Buttons.RestartButton).gameObject.BindEvent(_ => Managers.Scene.LoadScene(Define.EScene.GameScene));

		return true;
	}
}
