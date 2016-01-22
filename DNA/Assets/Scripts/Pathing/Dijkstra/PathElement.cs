using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace DNA {

	public enum DevelopmentState { 
		Undeveloped, 
		UnderConstruction, 
		Developed,
		Flooded,
		Damaged,
		UnderRepair,
		Abandoned
	}
}

namespace DNA.Paths {

	public delegate void OnSetObject (IPathElementObject obj);
	public delegate void OnSetState (DevelopmentState state);

	public class PathElement {

		DevelopmentState state = DevelopmentState.Undeveloped;
		public DevelopmentState State {
			get { return state; }
			set { 
				if (state != value) {
					state = value; 
					if (OnSetState != null)
						OnSetState (state);
				}
			}
		}

		IPathElementObject obj;
		public IPathElementObject Object {
			get { return obj; }
			set {
				obj = value;
				obj.Element = this;
				if (OnSetObject != null)
					OnSetObject (obj);
			}
		}

		public OnSetObject OnSetObject { get; set; }
		public OnSetState OnSetState { get; set; }

		static int version = 0;
		int myVersion = 0;
		List<IPathElementVisitor> visitors = new List<IPathElementVisitor> ();
		DevelopmentState stateBeforeFlood;

		public bool UpToDate { get { return myVersion == version; } }

		protected void UpdateVersion () {
			version ++;
		}

		public void SetUpToDate () {
			myVersion = version;
		}

		public void RegisterVisitor (IPathElementVisitor visitor) {
			// Debug.Log ("reg " + visitor);
			visitor.VisitorIndex = visitors.Count;
			visitors.Add (visitor);
		}

		public void RemoveVisitor (IPathElementVisitor visitor) {
			if (!visitors.Contains (visitor))
				return;
			// Debug.Log ("rem " + visitor);
			visitors.Remove (visitor);
			visitor.VisitorIndex = -1;
			for (int i = 0; i < visitors.Count; i ++) {
				visitors[i].VisitorIndex = i;
			}
		}
	}
}