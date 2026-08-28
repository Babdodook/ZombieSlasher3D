using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ZombieSlasher/Stage Table", fileName = "StageTable")]
public class StageTableData : ScriptableObject
{
	public List<StageData> Stages = new List<StageData>();
}
