using UnityEngine;
using System.Collections;

namespace GameActions {

	public class OccupyUnit : PerformerAction {

		public override string Name {
			get { return "OccupyUnit"; }
		}

		public override EnabledState EnabledState {
			get { return new OccupyUnitEnabledState (); }
		}

		public OccupyUnit () : base (-1, false, true) {}
	}
}