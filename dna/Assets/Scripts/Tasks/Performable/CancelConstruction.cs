using UnityEngine;
using System.Collections;
using DNA.Paths;
using DNA.InputSystem;

namespace DNA.Tasks {

	public class CancelConstruction : PerformerTask {

		PathElementContainer container;

		public override bool Enabled {
			get { return true; }
		}

		public CancelConstruction (PathElementContainer container) {
			this.container = container;
		}

		protected override void OnEnd () {
			container.CancelConstruction ();
			SelectionHandler.Clear ();
			base.OnEnd ();
		}
	}
}