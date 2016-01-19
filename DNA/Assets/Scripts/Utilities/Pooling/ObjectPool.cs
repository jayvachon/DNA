using UnityEngine;
using System.Collections;
using System.Collections.Generic;

#if UNITY_EDITOR
using System.IO;
using UnityEditor;
#endif

public class ObjectPool {

	static readonly Dictionary<string, ObjectPool> pools = new Dictionary<string, ObjectPool> ();
	Stack<MonoBehaviour> inactive = new Stack<MonoBehaviour> ();
	List<MonoBehaviour> active = new List<MonoBehaviour> ();

	MonoBehaviour prefab;

	#if UNITY_EDITOR
	[MenuItem ("Object Pool/Refresh Resources")]
	static void RefreshResources () {
		// TODO: for each asset in resources/prefabs, delete the asset and re-copy the asset in the project directory ONLY IF the asset exists in the project directory
		// (right now this is just deleting all the prefabs so that they can be re-created at runtime)
		PoolIOHandler.DeletePrefabsInResources ();
	}
	#endif

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
			m = MonoBehaviour.Instantiate (prefab) as MonoBehaviour;
		}

		active.Add (m);
		m.gameObject.SetActive (true);

		return m;
	}

	void ReleaseInstance (MonoBehaviour instance) {
		instance.gameObject.SetActive (false);
		active.Remove (instance);
		inactive.Push (instance);
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

	public static void Destroy (string id) {
		ObjectPool op = GetPool (id);
		if (op.active.Count > 0)
			op.ReleaseInstance (op.active[0]);
	}

	public static void Destroy (GameObject go) {
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

	public static List<T> GetActiveObjects<T> () where T : MonoBehaviour {
		return GetPool<T> ().active.ConvertAll (x => (T)x);
	}
}

public static class PoolIOHandler {

	static string ApplicationPath {
		get { return Application.dataPath; }
	}

	static string ResourcesPath {
		get { return ApplicationPath + "/Resources/Prefabs/"; }
	}

	static string Path {
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
		return Resources.Load (Path + id, typeof (MonoBehaviour)) as MonoBehaviour;
	}

	#if UNITY_EDITOR
	public static void DeletePrefabsInResources () {
		string[] files = Directory.GetFiles (ResourcesPath, "*.prefab");
		foreach (string f in files)
			AssetDatabase.DeleteAsset ("Assets" + f.Replace (Application.dataPath, ""));
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
