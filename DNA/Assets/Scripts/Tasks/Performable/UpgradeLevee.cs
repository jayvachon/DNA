using UnityEngine;
using System.Collections;

namespace DNA.Tasks {

	public class UpgradeLevee : CostTask {

		protected override void OnEnd () {
			Purchase ();
		}
	}
}