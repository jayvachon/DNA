using UnityEngine;
using System.Collections;

// deprecate
namespace DNA.Tasks {

	public class UpgradeFogOfWar : CostTask {

		protected override void OnEnd () {
			Purchase ();
			// FogOfWar.UpgradeFadeLevel ();
		}
	}
}