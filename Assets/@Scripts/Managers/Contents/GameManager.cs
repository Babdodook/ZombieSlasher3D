using UnityEngine;
using static Define;

public class GameManager
{
	public EGameState State { get; private set; } = EGameState.Ready;
	public float ElapsedTime { get; private set; }
	public int KillCount { get; private set; }
	public StageData SelectedStage { get; private set; }

	public void SetSelectedStage(StageData stage)
	{
		SelectedStage = stage;
	}

	public void Init()
	{
		State = EGameState.Playing;
		ElapsedTime = 0f;
		KillCount = 0;
	}

	public void Clear()
	{
		State = EGameState.Ready;
		ElapsedTime = 0f;
		KillCount = 0;
	}

	public void OnUpdate()
	{
		if (State != EGameState.Playing)
			return;

		ElapsedTime += Time.deltaTime;
	}

	public void AddKill()
	{
		KillCount++;
	}

	public void SetState(EGameState state)
	{
		State = state;
	}
}
