using UnityEngine;
using System.Collections;

public class PoolManager : MonoBehaviour {

	public ObjectPool objectPool;
	public Transform[] prefabs;

	void Awake () {
		for (int i = 0; i < prefabs.Length; i ++) {
			CreatePool (prefabs[i]);
		}
		CreateObjects ();
	}

	void CreatePool (Transform prefab) {
		string prefabName = prefab.name;
		ObjectPool op = Instantiate (objectPool) as ObjectPool;
		op.gameObject.name = prefabName + "Pool";
		op.Init (prefabName, prefab);
	}

	void CreateObjects () {
		ObjectPool.Instantiate ("MilkPool", new Vector3 (10, 0.5f, 8));
		ObjectPool.Instantiate ("Pasture", new Vector3 (0, 0.5f, 0));
		ObjectPool.Instantiate ("Pasture", new Vector3 (-6, 0.5f, 6));
		ObjectPool.Instantiate ("Pasture", new Vector3 (6, 0.5f, 6));
		ObjectPool.Instantiate ("Pasture", new Vector3 (0, 0.5f, 8));
		ObjectPool.Instantiate ("Hospital", new Vector3 (-8, 0.5f, -1));
		ObjectPool.Instantiate ("House", new Vector3 (8, 0.5f, -1));
		ObjectPool.Instantiate ("IceCreamCollector", new Vector3 (-4, 0.5f, -4));
		ObjectPool.Instantiate ("IceCreamCollector", new Vector3 (4, 0.5f, -4));
		ObjectPool.Instantiate ("ElderTransfer", new Vector3 (0, 0.5f, -6));
		ObjectPool.Instantiate ("MilkTransfer", new Vector3 (12, 0.5f, 8));
	}
}
