using UnityEngine;
using System.Collections;
using GameInput;

namespace Pathing {

	// Deprecated ?
	public class Handle : MonoBehaviour, IPoolable, IClickable {
		
		public static Handle Create (Transform parent) {
			GameObject go = ObjectPool.Instantiate ("Handle", parent.position).gameObject;
			go.transform.SetParent (parent);
			return go.GetScript<Handle> ();
		}

		public void Click (bool left) {}
		public void Drag (bool left, Vector3 mousePosition) {}
		public void Release (bool left) {}

		public void OnCreate () {}
		public void OnDestroy () {}
	}
}