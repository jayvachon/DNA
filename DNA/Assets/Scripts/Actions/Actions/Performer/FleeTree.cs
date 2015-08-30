using UnityEngine;
using System.Collections;

namespace GameActions {

	public class FleeTree : PerformerAction {

		public override string Name {
			get { return "FleeTree"; }
		}

		public FleeTree () : base (0f) {}

		public override void OnEnd () {
			// Go to next tree / Reset the tree
			Application.LoadLevel (0);
		}
	}
}