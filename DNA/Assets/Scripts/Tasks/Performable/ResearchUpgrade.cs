using UnityEngine;
using System.Collections;

namespace DNA.Tasks {

	public class ResearchUpgrade<T> : CostTask where T : Upgrade {

		public ResearchUpgrade () : base () {
			Settings.Duration = TotalCost;
		}
	}
}