using UnityEngine;
using System.Collections;
using GameInventory;

namespace DNA.Tasks {

	public class GenerateItem<T> : InventoryTask<T> where T : ItemHolder {

		public override bool Enabled {
			get { return !Holder.Full; }
		}

		protected override void OnEnd () {
			Holder.Add ();
			base.OnEnd ();
		}
	}
}