using UnityEngine;
using System.Collections;

namespace DNA.Tasks {

	public class EnabledTest : PerformerTask {

		public bool enabled;
		public override bool Enabled {
			get { return enabled; }
		}
	}
}