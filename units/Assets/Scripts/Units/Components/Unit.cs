using UnityEngine;
using System.Collections;

namespace Units {

	public class Unit : MonoBehaviour {
		
		public UnitRenderer unitRenderer;
		public UnitTransform unitTransform;

		public Transform Transform {
			get { return unitTransform.MyTransform; }
		}

		public virtual void OnSelect () {
			unitRenderer.OnSelect ();
			unitTransform.OnSelect ();
		}

		public virtual void OnUnselect () {
			unitRenderer.OnUnselect ();
			unitTransform.OnUnselect ();
		}
	}
}