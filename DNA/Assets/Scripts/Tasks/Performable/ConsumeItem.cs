using UnityEngine;
using System.Collections;
using InventorySystem;

namespace DNA.Tasks {

	public class ConsumeItem<T> : InventoryTask<T> where T : ItemGroup {

		public override bool Enabled {
			get { return !Group.Empty; }
		}

		protected override void OnEnd () {
			Group.Remove ();
			base.OnEnd ();
		}
	}
}