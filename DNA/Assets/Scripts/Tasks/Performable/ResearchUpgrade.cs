using UnityEngine;
using System.Collections;

namespace DNA.Tasks {

	public class ResearchUpgrade<T> : UpgradeTask where T : Upgrade {

		public override int Level {
			get { return Upgrades.Instance.GetUpgrade<T> ().CurrentLevel; }
		}

		public ResearchUpgrade () : base () {
			Settings.Duration = TotalCost;
		}

		protected override void OnStart () {
			Purchase ();
			base.OnStart ();
		}

		protected override void OnEnd () {
			Upgrades.Instance.NextLevel<T> ();
			base.OnEnd ();
		}
	}
}