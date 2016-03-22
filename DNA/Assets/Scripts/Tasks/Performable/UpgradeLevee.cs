using UnityEngine;
using System.Collections;

namespace DNA.Tasks {

	public class UpgradeLevee : UpgradeTask {

		Levee levee = null;
		Levee Levee {
			get {
				if (levee == null)
					levee = GameObject.Find ("Levee").gameObject.GetComponent<MonoBehaviour> () as Levee;
				return levee;
			}
		}

		protected override void OnEnd () {
			Purchase ();
			Levee.UpgradeHeight ();
		}
	}
}