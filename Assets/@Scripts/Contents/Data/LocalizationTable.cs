using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LocalizationEntry
{
	public string Key;
	[TextArea] public string Korean;
	[TextArea] public string English;
}

[CreateAssetMenu(menuName = "ZombieSlasher/Localization Table", fileName = "LocalizationTable")]
public class LocalizationTable : ScriptableObject
{
	public List<LocalizationEntry> Entries = new List<LocalizationEntry>();
}
