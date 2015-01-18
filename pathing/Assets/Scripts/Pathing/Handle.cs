using UnityEngine;
using System.Collections;
using GameInput;

namespace Pathing {

	public class Handle : MonoBehaviour, IPoolable, IClickable {
		
		public static Handle Create (Transform parent) {
			GameObject go = ObjectPool.Instantiate ("Handle", parent.position).gameObject;
			go.transform.SetParent (parent);
			return go.GetScript<Handle> ();
		}

		public void Click (ClickSettings settings) {
		
		}

		public void OnCreate () {}
		public void OnDestroy () {}
	}
}