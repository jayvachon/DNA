using UnityEngine;
using System.Collections;

namespace DNA.Tasks {

	// TODO: have this inherit from ResearchUpgrade (not doing it now bc doing so would set the duration of the task)
	public class UpgradeLevee : UpgradeTask {

		Levee levee = null;
		Levee Levee {
			get {
				if (levee == null)
					levee = GameObject.Find ("Levee").gameObject.GetComponent<MonoBehaviour> () as Levee;
				return levee;
			}
		}

		public override int Level {
			get { return Upgrades.Instance.GetUpgrade<LeveeHeight> ().CurrentLevel; }
		}

		protected override void OnEnd () {
			Purchase ();
			Upgrades.Instance.NextLevel<LeveeHeight> ();
		}
	}
}