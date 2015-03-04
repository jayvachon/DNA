using UnityEngine;
using System.Collections;

namespace Units {

	public class Unit : MonoBehaviour, IPoolable {
		
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

		public virtual void OnCreate () {}
		public virtual void OnDestroy () {}
	}
}