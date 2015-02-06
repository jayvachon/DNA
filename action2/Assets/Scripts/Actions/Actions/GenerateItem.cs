using UnityEngine;
using System.Collections;
using GameInventory;

namespace GameActions {

	public class GenerateItem<T> : Action where T : Item {

		public GenerateItem (float duration) : base (duration) {}
		
	}
}