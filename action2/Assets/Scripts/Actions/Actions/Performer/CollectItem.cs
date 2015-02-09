using UnityEngine;
using System.Collections;
using GameInventory;

namespace GameActions {

	public class CollectItem<T> : PerformerAction where T : ItemHolder {

		public override string Name {
			get { return "Collect " + (typeof (T)); }
		}

		public CollectItem (float duration) : base (duration) {}
		
	}
}