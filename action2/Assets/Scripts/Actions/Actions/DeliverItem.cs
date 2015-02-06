using UnityEngine;
using System.Collections;
using GameInventory;

namespace GameActions {

	public class DeliverItem<T> : Action where T : Item {

		public DeliverItem (float duration) : base (duration) {}
		
	}
}