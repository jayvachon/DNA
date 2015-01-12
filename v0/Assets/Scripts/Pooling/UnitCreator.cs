using UnityEngine;
using System.Collections;

public class UnitCreator : MonoBehaviour {

	void Start () {
		ObjectPool.Instantiate ("Pasture", new Vector3 (0, 0.5f, 0));
		ObjectPool.Instantiate ("Pasture", new Vector3 (-4, 0.5f, 4));
		ObjectPool.Instantiate ("Pasture", new Vector3 (4, 0.5f, 4));
		ObjectPool.Instantiate ("Pasture", new Vector3 (0, 0.5f, 8));
		ObjectPool.Instantiate ("IceCreamCollector", new Vector3 (-2, 0.5f, -2));
	}
}
