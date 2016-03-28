using UnityEngine;
using System.Collections;

namespace DNA.Tasks {

	public class ResearchUpgrade<T> : UpgradeTask where T : Upgrade {

		public override int Level {
			get { 
				T t;
				if (Upgrades.Instance.TryGetUpgrade<T> (out t)) {
					return t.CurrentLevel;
				}
				return 0;
			}
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