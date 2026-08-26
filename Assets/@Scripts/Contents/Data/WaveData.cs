using System;

[Serializable]
public class WaveData
{
	public string MonsterPrefabKey;
	public float StartTime;
	public float SpawnInterval = 1f;
	public int SpawnCount = -1;
	public int MaxAliveCap = 30;
}
