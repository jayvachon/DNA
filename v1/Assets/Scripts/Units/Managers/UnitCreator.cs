using UnityEngine;
using System.Collections;

public class UnitCreator : MonoBehaviour {

	static UnitCreator instanceInternal = null;
	static public UnitCreator instance {
		get {
			if (instanceInternal == null) {
				instanceInternal = Object.FindObjectOfType (typeof (UnitCreator)) as UnitCreator;
				if (instanceInternal == null) {
					GameObject go = new GameObject ("UnitCreator");
					DontDestroyOnLoad (go);
					instanceInternal = go.AddComponent<UnitCreator>();
				}
			}
			return instanceInternal;
		}
	}

	public Transform Create<T> (Vector3 position) where T : Unit {
		string name = typeof (T).Name;
		name = name.Substring (0, name.Length);
		if (ObjectPool.GetPool (name) == null) {
			CreatePool<T> ();
		}
		return ObjectPool.Instantiate (name, position);
	}

	void CreatePool<T> () where T : Unit {
		string prefabName = typeof (T).Name;
		GameObject go = new GameObject (prefabName);
		DontDestroyOnLoad (go);
		go.AddComponent<ObjectPool> ().Init (prefabName, UnitsList.instance.GetUnit (prefabName).transform);
	}
}
