using UnityEngine;
using System.Collections;
using InventorySystem;

namespace DNA.Tasks {

	public class CollectItem<T> : InventoryTask<T> where T : ItemGroup {

		public override bool Enabled {
			get { return !Group.Full; }
		}

		public CollectItem (string symbolOverride="") : base (symbolOverride) {}

		protected override void OnEnd () {
			AcceptorInventory.Get<T> ().Transfer (Group);
			base.OnEnd ();
		}
	}
}