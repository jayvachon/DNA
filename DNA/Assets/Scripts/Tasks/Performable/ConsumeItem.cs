using UnityEngine;
using System.Collections;
using DNA.InventorySystem;

namespace DNA.Tasks {

	public class ConsumeItem<T> : InventoryTask<T> where T : ItemHolder {

		public override bool Enabled {
			get { return !Holder.Empty; }
		}

		protected override void OnEnd () {
			Holder.Remove ();
			base.OnEnd ();
		}
	}
}