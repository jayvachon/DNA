using UnityEngine;
using System.Collections;

public interface IPoolable {
	void OnPoolCreate ();
	void OnPoolDestroy ();
}
