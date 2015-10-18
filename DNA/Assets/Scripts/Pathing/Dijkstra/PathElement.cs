using UnityEngine;
using System.Collections;

namespace DNA {

	public enum DevelopmentState { 
		Undeveloped, 
		UnderConstruction, 
		Developed,
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
				state = value; 
				if (OnSetState != null)
					OnSetState (state);
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

		public bool UpToDate { get { return myVersion == version; } }

		protected void UpdateVersion () {
			version ++;
		}

		public void SetUpToDate () {
			myVersion = version;
		}
	}
}