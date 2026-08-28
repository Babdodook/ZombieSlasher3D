using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class LocalizedText : MonoBehaviour
{
	[SerializeField] private string _key;

	private TMP_Text _text;

	private void Awake()
	{
		_text = GetComponent<TMP_Text>();
	}

	private void OnEnable()
	{
		Refresh();
		Managers.Localization.OnLanguageChanged += Refresh;
	}

	private void OnDisable()
	{
		if (Managers.Localization != null)
			Managers.Localization.OnLanguageChanged -= Refresh;
	}

	private void Refresh()
	{
		_text.text = Managers.Localization.Get(_key);
	}
}
