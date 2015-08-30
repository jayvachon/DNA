using UnityEngine;
using System.Collections;

namespace GameActions {

	public class AcceptOccupyUnitEnabledState : EnabledState {

		public override bool Enabled { get { return true; } }
		public override string RequiredPair { get { return ""; } }
	}
}