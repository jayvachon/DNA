using UnityEngine;
using System.Collections;

public interface IPoolable {
	void OnCreate ();
	void OnDestroy ();
}
