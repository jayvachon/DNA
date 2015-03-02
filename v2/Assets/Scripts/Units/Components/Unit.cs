using UnityEngine;
using System.Collections;

namespace Units {

	public class Unit : MonoBehaviour {
		
		public UnitTransform unitTransform;
		public UnitRenderer unitRenderer;

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