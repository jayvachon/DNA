using UnityEngine;
using System.Collections;
using Units;

namespace GameActions {

	// not needed?
	public class CollectUnit<T> : PerformerAction where T : Unit {

		public CollectUnit (float duration) : base (duration) {}
	}
}