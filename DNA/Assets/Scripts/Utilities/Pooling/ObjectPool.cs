using UnityEngine;
using System.Collections;
using System.Collections.Generic;

#if UNITY_EDITOR
using System.IO;
using System.Linq;
using UnityEditor;
#endif

public class ObjectPool {

	static readonly Dictionary<string, ObjectPool> pools = new Dictionary<string, ObjectPool> ();
	Stack<MonoBehaviour> inactive = new Stack<MonoBehaviour> ();
	List<MonoBehaviour> active = new List<MonoBehaviour> ();

	MonoBehaviour prefab;

	public void Init<T> (string id) where T : MonoBehaviour {
		prefab = PoolIOHandler.LoadPrefab<T> (id);
	}

	static ObjectPool GetPool (string id) {
		return GetPool<MonoBehaviour> (id);
	}

	static ObjectPool GetPool<T> () where T : MonoBehaviour {
		return GetPool<T> (typeof (T).Name);
	}

	static ObjectPool GetPool<T> (string id) where T : MonoBehaviour {
		ObjectPool op;
		if (pools.TryGetValue (id, out op)) {
			return op;
		} else {
			return CreatePool<T> (id);
		}
	}

	static ObjectPool CreatePool<T> (string id) where T : MonoBehaviour {
		ObjectPool newPool = new ObjectPool ();
		newPool.Init<T> (id);
		pools.Add (id, newPool);
		return newPool;
	}

	MonoBehaviour CreateInstance () {

		MonoBehaviour m;

		if (inactive.Count > 0) {
			m = inactive.Pop ();
		} else {
			try {
				m = MonoBehaviour.Instantiate (prefab) as MonoBehaviour;
			} catch {
				throw new System.Exception ("Could not find a prefab of type " + prefab);
			}
		}

		active.Add (m);
		m.gameObject.SetActive (true);
		m.transform.SetParent (null);
		#if UNITY_EDITOR
		m.Log ("Instantiated");
		#endif

		return m;
	}

	void ReleaseInstance (MonoBehaviour instance) {
		// instance.transform.SetParent (InactiveInstancesContainer.Instance.Transform);
		if (!instance.gameObject.activeSelf)
			return;
			
		instance.gameObject.SetActive (false);
		active.Remove (instance);
		inactive.Push (instance);
		#if UNITY_EDITOR
		instance.Log ("Destroyed");
		#endif
	}

	public static MonoBehaviour Instantiate (string id, Vector3 position=new Vector3 (), Quaternion rotation=new Quaternion ()) {
		MonoBehaviour m = GetPool (id).CreateInstance ();
		m.transform.position = position;
		m.transform.localRotation = rotation;
		return m;
	}

	public static T Instantiate<T> (Vector3 position=new Vector3 (), Quaternion rotation=new Quaternion ()) where T : MonoBehaviour {
		T t = GetPool<T> ().CreateInstance ().GetComponent<T> () as T;
		t.transform.position = position;
		t.transform.localRotation = rotation;
		return t;
	}

	// TODO: these appear to break if you've been using generic methods for e.g. instantiation (is it because (Clone) is being added to the name?)
	public static void Destroy (string id) {
		ObjectPool op = GetPool (id);
		if (op.active.Count > 0)
			op.ReleaseInstance (op.active[0]);
	}

	// TODO: these appear to break if you've been using generic methods for e.g. instantiation (is it because (Clone) is being added to the name?)
	public static void Destroy (GameObject go) {
		Debug.LogWarning ("Unstable - use generic methods instead");
		MonoBehaviour m = go.GetComponent<MonoBehaviour> ();
		ObjectPool op = GetPool (m.GetType ().Name);
		op.ReleaseInstance (m);
	}

	public static void Destroy (Transform t) {
		MonoBehaviour m = t.GetComponent<MonoBehaviour> ();
		ObjectPool op = GetPool (m.GetType ().Name);
		op.ReleaseInstance (m);
	}

	public static void Destroy<T> (T t) where T : MonoBehaviour {
		GetPool<T> ().ReleaseInstance (t);
	}

	public static void Destroy<T> (Transform t) where T : MonoBehaviour {
		GetPool<T> ().ReleaseInstance (t.GetComponent<MonoBehaviour> ());
	}

	public static void Destroy<T> (GameObject go) where T : MonoBehaviour {
		GetPool<T> ().ReleaseInstance (go.GetComponent<MonoBehaviour> ());
	}

	// TODO: these appear to break if you've been using generic methods for e.g. instantiation (is it because (Clone) is being added to the name?)
	public static void DestroyChildren (Transform t) {

		List<Transform> children = new List<Transform> ();
		foreach (Transform child in t) children.Add (child);

		for (int i = 0; i < children.Count; i ++)
			Destroy (children[i]);
	}

	public static void DestroyChildren<T> (Transform t, System.Action<T> onDestroy=null) where T : MonoBehaviour {

		List<Transform> children = new List<Transform> ();
		foreach (Transform child in t) children.Add (child);

		for (int i = 0; i < children.Count; i ++) {
			if (onDestroy != null)
				onDestroy (children[i].GetComponent<T> ());
			Destroy<T> (children[i]);
		}
	}

	public static void DestroyChildrenWithCriteria<T> (Transform t, System.Func<T, bool> criteria) where T : MonoBehaviour {

		List<T> children = new List<T> ();
		foreach (Transform child in t) children.Add (child.GetComponent<T> ());

		for (int i = 0; i < children.Count; i ++) {
			if (criteria (children[i])) Destroy<T> (children[i]);
		}
	}

	public static List<T> GetActiveInstances<T> () where T : MonoBehaviour {
		return GetPool<T> ().active.ConvertAll (x => (T)x);
	}
}

#if UNITY_EDITOR
[InitializeOnLoad]
#endif
public static class PoolIOHandler {

	static string ApplicationPath {
		get { return Application.dataPath; }
	}

	static string ResourcesPath {
		get { return ApplicationPath + "/Resources/Prefabs/"; }
	}

	static string PrefabsPath {
		get { return "Prefabs/"; }
	}

	/**
	  * Loads a prefab with the given id
	  * First tries to load the prefab from Resources
	  * Failing that, searches for the prefab in the project and copies it into the Resources folder
	  * Failing that, tries to create a new prefab with the given type T
	 */
	public static MonoBehaviour LoadPrefab<T> (string id) where T : MonoBehaviour {
		
		#if UNITY_EDITOR

		MonoBehaviour prefab = LoadMonoBehaviour (id);
		if (prefab == null) {
			string projectPath = FindPrefabDirectory (id, ApplicationPath);
			if (projectPath != "") {
				string p = "Assets" + projectPath.Replace (Application.dataPath, "");
				AssetDatabase.CopyAsset (p, "Assets/Resources/Prefabs/" + id + ".prefab");
				AssetDatabase.Refresh ();
			} else {
				AddPrefab<T> (id);
			}
			return LoadMonoBehaviour (id);
		} else {
			return prefab;
		}

		#else

		try {
			return LoadMonoBehaviour (id);
		} catch {
			throw new System.Exception ("No prefab named '" + id + "' exists in the Resources directory");
		}

		#endif
	}

	static MonoBehaviour LoadMonoBehaviour (string id) {
		return Resources.Load (PrefabsPath + id, typeof (MonoBehaviour)) as MonoBehaviour;
	}

	#if UNITY_EDITOR

	const string MONITOR_ITEM = "Object Pool/Monitor prefab changes";
	static bool monitorEnabled;

	static PoolIOHandler () {
		PoolIOHandler.monitorEnabled = EditorPrefs.GetBool (PoolIOHandler.MONITOR_ITEM, true);
		EditorApplication.delayCall += () => {
			SetMonitorEnabled (monitorEnabled);
		};
	}

	static void PrefabUpdated (GameObject go) {

		GameObject prefab = PrefabUtility.GetPrefabParent (go) as GameObject;
		string prefabPath = AssetDatabase.GetAssetPath (prefab);

		// Skip if the prefab being updated is in the Resources directory
		if (prefabPath.Contains ("Resources"))
			return;
		
		string prefabName = Path.GetFileNameWithoutExtension (prefabPath);
		string resourcePath = GetPrefabInResources (prefabName);
		
		// Only update if this prefab has been added to the Resources directory
		if (resourcePath != null) {
			AssetDatabase.DeleteAsset ("Assets" + resourcePath.Replace (Application.dataPath, ""));
			AssetDatabase.CopyAsset (prefabPath, "Assets/Resources/Prefabs/" + prefabName + ".prefab");
			AssetDatabase.Refresh ();
		}
	}

	public static void SetMonitorEnabled (bool enabled) {
		Menu.SetChecked (PoolIOHandler.MONITOR_ITEM, enabled);
		EditorPrefs.SetBool (PoolIOHandler.MONITOR_ITEM, enabled);
		PoolIOHandler.monitorEnabled = enabled;
		if (enabled)
			PrefabUtility.prefabInstanceUpdated += PrefabUpdated;
		else
			PrefabUtility.prefabInstanceUpdated -= PrefabUpdated;
	}

	[MenuItem (PoolIOHandler.MONITOR_ITEM)]
	static void ToggleMonitor () {
		SetMonitorEnabled (!PoolIOHandler.monitorEnabled);
	}

	[MenuItem ("Object Pool/Refresh Prefabs")]
	static void RefreshResources () {

		// Removes and replaces all the prefabs in the Resources directory
		// This will be very slow for a project with many prefabs...

		string[] files = GetPrefabsInResources ();
		foreach (string f in files) {

			string fileName = Path.GetFileNameWithoutExtension (f);
			string projectPath = FindPrefabDirectory (fileName, ApplicationPath);

			AssetDatabase.DeleteAsset ("Assets" + f.Replace (Application.dataPath, ""));

			string p = "Assets" + projectPath.Replace (Application.dataPath, "");

			AssetDatabase.CopyAsset (p, "Assets/Resources/Prefabs/" + fileName + ".prefab");
			AssetDatabase.Refresh ();
		}
	}
	
	public static void DeletePrefabsInResources () {
		string[] files = GetPrefabsInResources ();
		foreach (string f in files)
			AssetDatabase.DeleteAsset ("Assets" + f.Replace (Application.dataPath, ""));
	}

	public static string[] GetPrefabsInResources () {
		return Directory.GetFiles (ResourcesPath, "*.prefab");
	}

	public static string GetPrefabInResources (string prefabName) {
		return GetPrefabsInResources ()
			.ToList<string> ()
			.Find (x => x.Contains (prefabName + ".prefab"));
	}

	static string FindPrefabDirectory (string id, string path) {

		string[] directories = Directory.GetDirectories (path);

		// TODO: http://docs.unity3d.com/ScriptReference/AssetDatabase.FindAssets.html
		foreach (string d in directories) {
			string[] files = Directory.GetFiles (d, "*.prefab");
			foreach (string f in files) {
				if (f.Contains (id + ".prefab"))
					return f;
			}
			string s = FindPrefabDirectory (id, d);
			if (s != "") return s;
		}

		return "";
	}

	static void AddPrefab<T> (string id) where T : MonoBehaviour {
		GameObject go = new GameObject (id);
		PrefabUtility.CreatePrefab ("Assets/Resources/Prefabs/" + id + ".prefab", go);
		Object.DestroyImmediate (go);
	}
	#endif
}

public class InactiveInstancesContainer : MonoBehaviour {

	static InactiveInstancesContainer instance = null;
	static public InactiveInstancesContainer Instance {
		get {
			if (instance == null) {
				instance = UnityEngine.Object.FindObjectOfType (typeof (InactiveInstancesContainer)) as InactiveInstancesContainer;
				if (instance == null) {
					GameObject go = new GameObject ("InactiveInstancesContainer");
					go.hideFlags = HideFlags.HideInHierarchy;
					DontDestroyOnLoad (go);
					instance = go.AddComponent<InactiveInstancesContainer> ();
				}
			}
			return instance;
		}
	}

	Transform myTransform;
	public Transform Transform {
		get {
			if (myTransform == null)
				myTransform = transform;
			return myTransform;
		}
	}
}

public class Log : MonoBehaviour {
	
	public string message = "";
	public int gameObjectId;

	public void Newline (string msg) {
		message += "[" + Timestamp () + "] " + msg + "\n";
	}

	string Timestamp () {
		return System.DateTime.Now.ToString("HH:mm:ss");
	}
}


public static class LogExtensionMethods {

	public static void Log (this MonoBehaviour mb, string msg) {
		Log l = mb.LazyGetComponent<Log> ();
		l.gameObjectId = mb.gameObject.GetInstanceID ();
		l.Newline (msg);
	}
}