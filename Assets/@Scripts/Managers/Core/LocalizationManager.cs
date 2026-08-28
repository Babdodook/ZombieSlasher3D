using System;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class LocalizationManager
{
	private const string LanguagePrefsKey = "Language";
	private const string TableResourcePath = "LocalizationTable";

	private Dictionary<string, LocalizationEntry> _table = new Dictionary<string, LocalizationEntry>();

	public ELanguage CurrentLanguage { get; private set; } = ELanguage.Korean;
	public event Action OnLanguageChanged;

	public void Init()
	{
		LocalizationTable table = Resources.Load<LocalizationTable>(TableResourcePath);
		if (table != null)
		{
			foreach (LocalizationEntry entry in table.Entries)
				_table[entry.Key] = entry;
		}

		CurrentLanguage = LoadLanguagePref();
	}

	private ELanguage LoadLanguagePref()
	{
		if (PlayerPrefs.HasKey(LanguagePrefsKey))
			return (ELanguage)PlayerPrefs.GetInt(LanguagePrefsKey);

		return Application.systemLanguage == SystemLanguage.Korean ? ELanguage.Korean : ELanguage.English;
	}

	public void SetLanguage(ELanguage language)
	{
		if (CurrentLanguage == language)
			return;

		CurrentLanguage = language;
		PlayerPrefs.SetInt(LanguagePrefsKey, (int)language);
		OnLanguageChanged?.Invoke();
	}

	public string Get(string key)
	{
		if (_table.TryGetValue(key, out LocalizationEntry entry) == false)
			return key;

		return CurrentLanguage == ELanguage.Korean ? entry.Korean : entry.English;
	}
}
