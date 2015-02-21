using UnityEngine;
using System.Collections;

namespace Units {

	public class Unit : MonoBehaviour {
		
		public UnitRenderer unitRenderer;

		public virtual void OnSelect () {
			unitRenderer.OnSelect ();
		}

		public virtual void OnUnselect () {
			unitRenderer.OnUnselect ();
		}
	}
}