using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace GameInventory {

	public delegate bool ItemHasAttribute (Item item);
	public delegate void InventoryUpdated ();

	[System.Serializable]
	public class Inventory : System.Object {

		public readonly IInventoryHolder holder;
		public InventoryUpdated inventoryUpdated;

		List<ItemHolder> holders = new List<ItemHolder> ();
		public List<ItemHolder> Holders {
			get { return holders; }
		}

		public Inventory (IInventoryHolder holder) {
			this.holder = holder;
		}

		public void Add (ItemHolder holder) {
			holder.Inventory = this;
			holder.HolderUpdated += NotifyInventoryUpdated;
			holders.Add (holder);
			NotifyInventoryUpdated ();
		}

		public List<Item> AddItem<T> () where T : ItemHolder {
			return Get<T> ().Add ();
		}

		public List<Item> AddItem<T> (Item item) where T : ItemHolder {
			return Get<T> ().Add (item);
		}

		public List<Item> AddItems<T> (List<Item> items) where T : ItemHolder {
			return Get<T> ().Add (items);
		}

		public void AddItems<T> (int amount) where T : ItemHolder {
			T holder = Get<T> ();
			for (int i = 0; i < amount; i ++) {
				holder.Add ();
			}
		}

		public List<Item> RemoveItem<T> () where T : ItemHolder {
			return Get<T> ().Remove ();
		}

		public List<Item> RemoveItems<T> (int amount) where T : ItemHolder {
			return Get<T> ().Remove (amount);
		}

		public void Empty () {
			foreach (ItemHolder holder in holders) {
				holder.Clear ();
			}
		}

		public T Get<T> () where T : ItemHolder {
			foreach (ItemHolder holder in holders) {
				T t = holder as T;
				if (t != null)
					return t;
			}
			return null;
		}

		public ItemHolder Get (string name) {
			foreach (ItemHolder holder in holders) {
				if (holder.Name == name)
					return holder;		
			}
			return null;
		}

		public bool Has<T> () {
			foreach (ItemHolder holder in holders) {
				if (holder is T)
					return true;
			}
			return false;
		}

		public void Transfer<T> (Inventory boundInventory, int amount=-1, ItemHasAttribute transferable=null) where T : ItemHolder {
			T sender = boundInventory.Get<T> ();
			T receiver = Get<T> ();
			receiver.Transfer (sender, amount, transferable);
		}

		public void NotifyInventoryUpdated () {
			if (inventoryUpdated != null) {
				inventoryUpdated ();
			}
		}

		/**
		 *	Debugging
		 */

		public virtual void Print () {
			foreach (ItemHolder holder in holders) {
				holder.Print ();
			}
		}
	}
}