using UnityEngine;
using System.Collections;
using DNA.Paths;
using DNA.InputSystem;

namespace DNA.Tasks {

	public class DemolishUnit : PerformerTask {

		public override bool Enabled {
			get { return true; }
		}

		PathElementContainer container;

		public DemolishUnit (PathElementContainer container) {
			this.container = container;
		}

		protected override void OnEnd () {
			container.Demolish ();
			SelectionHandler.Clear ();
			base.OnEnd ();
		}
	}
}