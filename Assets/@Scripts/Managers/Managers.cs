using UnityEngine;

public class Managers : MonoBehaviour
{
	private static Managers s_instance;
	private static Managers Instance { get { Init(); return s_instance; } }

	#region Contents
	private GameManager _game = new GameManager();
	private StageManager _stage = new StageManager();

	public static GameManager Game { get { return Instance?._game; } }
	public static StageManager Stage { get { return Instance?._stage; } }
	#endregion

	#region Core
	private PoolManager _pool = new PoolManager();
	private ResourceManager _resource = new ResourceManager();
	private SceneManagerEx _scene = new SceneManagerEx();
	private SoundManager _sound = new SoundManager();
	private UIManager _ui = new UIManager();

	public static PoolManager Pool { get { return Instance?._pool; } }
	public static ResourceManager Resource { get { return Instance?._resource; } }
	public static SceneManagerEx Scene { get { return Instance?._scene; } }
	public static SoundManager Sound { get { return Instance?._sound; } }
	public static UIManager UI { get { return Instance?._ui; } }
	#endregion

	public static void Init()
	{
		if (s_instance == null)
		{
			GameObject go = GameObject.Find("@Managers");
			if (go == null)
			{
				go = new GameObject { name = "@Managers" };
				go.AddComponent<Managers>();
			}

			DontDestroyOnLoad(go);

			s_instance = go.GetComponent<Managers>();
			s_instance._sound.Init();
		}
	}

	public static void Clear()
	{
		Sound.Clear();
		UI.Clear();
		Pool.Clear();
		Scene.Clear();
		Stage.Clear();
	}
}
