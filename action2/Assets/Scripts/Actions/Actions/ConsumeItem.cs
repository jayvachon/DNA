using UnityEngine;
using System.Collections;
using GameInventory;

namespace GameActions {

	public class ConsumeItem<T> : Action where T : Item {

		public ConsumeItem (float duration) : base (duration) {}
		
	}
}