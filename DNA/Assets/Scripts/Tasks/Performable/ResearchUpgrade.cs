using UnityEngine;
using System.Collections;

namespace DNA.Tasks {

	public class ResearchUpgrade<T> : CostTask where T : Upgrade {

		public ResearchUpgrade () : base () {
			Settings.Duration = TotalCost;
		}

		protected override void OnStart () {
			Purchase ();
			base.OnStart ();
		}

		protected override void OnEnd () {
			Upgrades.Instance.SetLevel<T> ();
			base.OnEnd ();
		}
	}
}