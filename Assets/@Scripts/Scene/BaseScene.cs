using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;
using static Define;

public abstract class BaseScene : InitBase
{
	public EScene SceneType { get; protected set; } = Define.EScene.Unknown;

	public override bool Init()
	{
		if (base.Init() == false)
			return false;

		EventSystem eventSystem = GameObject.FindAnyObjectByType<EventSystem>();
		if (eventSystem == null)
		{
			GameObject go = new GameObject() { name = "@EventSystem" };
			eventSystem = go.AddComponent<EventSystem>();
			go.AddComponent<InputSystemUIInputModule>();
		}
		else if (eventSystem.GetComponent<InputSystemUIInputModule>() == null)
		{
			StandaloneInputModule legacyModule = eventSystem.GetComponent<StandaloneInputModule>();
			if (legacyModule != null)
				Object.Destroy(legacyModule);

			eventSystem.gameObject.AddComponent<InputSystemUIInputModule>();
		}

		return true;
	}

	public abstract void Clear();
}
