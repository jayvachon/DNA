using UnityEngine;
using System.Collections;
using GameInventory;

namespace GameActions {

	public abstract class PerformCondition {

		// This is checked before the action is performed
		// if false is returned the action is not performed
		public abstract bool CanPerform { get; }

	}
}