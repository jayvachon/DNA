using UnityEngine;
using System.Collections;

namespace GameInventory {

	public class Item {

		public ItemHolder Holder { get; set; }
		public Inventory Inventory { get { return Holder.Inventory; } }

		public virtual void OnAdd () {}
		public virtual void Print () {}
	}
}