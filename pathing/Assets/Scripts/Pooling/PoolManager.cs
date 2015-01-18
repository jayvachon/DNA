using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PoolManager : MonoBehaviour {

	public static PoolManager instance = null;
	public ObjectPool objectPool;
	public Transform[] prefabs;

	void Awake () {
		
		if (instance == null)
			instance = this;

		for (int i = 0; i < prefabs.Length; i ++) {
			CreatePool (prefabs[i]);
		}
		CreateObjects ();
	}

	/*public void CreateUnit<T> (Vector3 position) where T : Unit {
		string name = typeof (T).Name;
		name = name.Substring (0, name.Length-4);
		ObjectPool.Instantiate (name, position);
	}*/

	void CreatePool (Transform prefab) {
		string prefabName = prefab.name;
		ObjectPool op = Instantiate (objectPool) as ObjectPool;
		op.gameObject.name = prefabName + "Pool";
		op.Init (prefabName, prefab);
	}

	void CreateObjects () {
		
	}
}
