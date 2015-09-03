using UnityEngine;
using System.Collections;
using GameInventory;

namespace DNA.Tasks {

	public class FleeTree : CostTask {

		protected override void OnEnd () {
			Application.LoadLevel (0);
			base.OnEnd ();
		}
	}
}