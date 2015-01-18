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
		ObjectPool.Instantiate ("MilkshakeMaker", new Vector3 (0, 0.5f, 0));
		ObjectPool.Instantiate ("Pasture", new Vector3 (-4, 0.5f, 4));
		ObjectPool.Instantiate ("MilkPool", new Vector3 (4, 0.5f, 4));
		ObjectPool.Instantiate ("Tree", new Vector3 (0, 1, -6));
		/*ObjectPool.Instantiate ("MilkPool", new Vector3 (10, 0.5f, 8));
		ObjectPool.Instantiate ("MilkshakeMaker", new Vector3 (4, 0.5f, 0));
		ObjectPool.Instantiate ("MilkshakeTransfer", new Vector3 (6, 0.5f, 0));
		ObjectPool.Instantiate ("TransferUnit", new Vector3 (-6, 0.5f, 0));
		ObjectPool.Instantiate ("Pasture", new Vector3 (-4, 0.5f, 0));
		ObjectPool.Instantiate ("Pasture", new Vector3 (-6, 0.5f, 6));
		ObjectPool.Instantiate ("Pasture", new Vector3 (6, 0.5f, 6));
		ObjectPool.Instantiate ("Pasture", new Vector3 (0, 0.5f, 8));
		ObjectPool.Instantiate ("Hospital", new Vector3 (-8, 0.5f, -1));
		ObjectPool.Instantiate ("House", new Vector3 (8, 0.5f, -1));
		ObjectPool.Instantiate ("IceCreamCollector", new Vector3 (-4, 0.5f, -4));
		ObjectPool.Instantiate ("IceCreamCollector", new Vector3 (4, 0.5f, -4));
		ObjectPool.Instantiate ("ElderTransfer", new Vector3 (0, 0.5f, -6));
		ObjectPool.Instantiate ("MilkTransfer", new Vector3 (12, 0.5f, 8));*/
	}
}
