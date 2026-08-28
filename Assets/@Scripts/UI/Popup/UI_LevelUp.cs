using System.Collections.Generic;
using UnityEngine;

public class UI_LevelUp : UI_Popup
{
	private const int CardCount = 3;

	private enum GameObjects
	{
		Card0,
		Card1,
		Card2,
	}

	private enum Buttons
	{
		Card0Button,
		Card1Button,
		Card2Button,
	}

	private enum Images
	{
		Card0Icon,
		Card1Icon,
		Card2Icon,
	}

	private enum Texts
	{
		Card0Name,
		Card1Name,
		Card2Name,
		Card0Desc,
		Card1Desc,
		Card2Desc,
	}

	public override bool Init()
	{
		if (base.Init() == false)
			return false;

		BindObjects(typeof(GameObjects));
		BindButtons(typeof(Buttons));
		BindImages(typeof(Images));
		BindTexts(typeof(Texts));

		return true;
	}

	public void Setup(List<SkillData> choices)
	{
		for (int i = 0; i < CardCount; i++)
		{
			GameObject card = GetObject(i);

			if (i >= choices.Count)
			{
				card.SetActive(false);
				continue;
			}

			card.SetActive(true);

			SkillData skill = choices[i];
			GetImage(i).sprite = skill.Icon;
			GetText(i).text = skill.GetDisplayName();
			GetText(CardCount + i).text = string.Format(skill.GetDescription(), skill.MagnitudePerLevel);

			GetButton(i).gameObject.BindEvent(_ => OnChoiceSelected(skill));
		}
	}

	private void OnChoiceSelected(SkillData skill)
	{
		Managers.Level.ApplySkillChoice(skill);
		ClosePopupUI();

		// Multiple level-ups from one XP pickup stack several of these popups; only
		// resume the game once the last one in the stack has been resolved.
		if (Managers.UI.GetPopupCount() == 0)
			Time.timeScale = 1f;
	}
}
