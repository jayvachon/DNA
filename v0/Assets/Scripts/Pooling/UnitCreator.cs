using UnityEngine;
using System.Collections;

public class UnitCreator : MonoBehaviour {

	// TODO: rename this to "PoolManager" and have it create pools programmatically at the start of the game
	void Start () {
		ObjectPool.Instantiate ("Pasture", new Vector3 (0, 0.5f, 0));
		ObjectPool.Instantiate ("Pasture", new Vector3 (-4, 0.5f, 4));
		ObjectPool.Instantiate ("Pasture", new Vector3 (4, 0.5f, 4));
		ObjectPool.Instantiate ("Pasture", new Vector3 (0, 0.5f, 8));
		ObjectPool.Instantiate ("IceCreamCollector", new Vector3 (-2, 0.5f, -2));
		ObjectPool.Instantiate ("IceCreamCollector", new Vector3 (2, 0.5f, -2));
		ObjectPool.Instantiate ("Hospital", new Vector3 (-6, 0.5f, -1));
	}
}
