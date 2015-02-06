using UnityEngine;
using System.Collections;
using GameInventory;

namespace GameActions {

	public class CollectItem<T> : Action where T : Item {

		public CollectItem (float duration) : base (duration) {}
		
	}
}