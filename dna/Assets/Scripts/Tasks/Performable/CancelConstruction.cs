using UnityEngine;
using System.Collections;
using DNA.Paths;
using DNA.InputSystem;

namespace DNA.Tasks {

	public class CancelConstruction : PerformerTask {

		public PathElementContainer Container { get; set; }

		public override bool Enabled {
			get { return true; }
		}

		protected override void OnEnd () {
			Container.CancelConstruction ();
			SelectionHandler.Clear ();
			base.OnEnd ();
		}
	}
}