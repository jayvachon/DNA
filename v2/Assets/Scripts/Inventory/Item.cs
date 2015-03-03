using UnityEngine;
using System.Collections;

namespace GameInventory {

	public class Item {

		public ItemHolder Holder { get; set; }

		public virtual void Print () {
			Debug.Log ("");
		}
	}
}