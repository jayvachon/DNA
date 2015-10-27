using UnityEngine;
using System.Collections;
using InventorySystem;

namespace DNA.Tasks {

	public class CollectItem<T> : InventoryTask<T> where T : ItemGroup {

		public override bool Enabled {
			get { return !Group.Full; }
		}

		protected override void OnEnd () {
			AcceptorInventory.Get<T> ().Transfer (Group);
			base.OnEnd ();
		}
	}
}