using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace GameInventory {

	public delegate bool ItemHasAttribute (Item item);

	public class Inventory {

		List<ItemHolder> holders = new List<ItemHolder> ();
		public List<ItemHolder> Holders {
			get { return holders; }
		}

		public void Add (ItemHolder holder) {
			holders.Add (holder);
		}

		public List<Item> AddItem<T> (Item item) where T : ItemHolder {
			T holder = Get<T> () as T;
			return holder.Add (item);
		}

		public List<Item> AddItems<T> (List<Item> items) where T : ItemHolder {
			T holder = Get<T> () as T;
			return holder.Add (items);
		}

		public List<Item> RemoveItem<T> () where T : ItemHolder {
			T holder = Get<T> () as T;
			return holder.Remove ();
		}

		public List<Item> RemoveItems<T> (int amount) where T : ItemHolder {
			T holder = Get<T> () as T;
			return holder.Remove (amount);
		}

		public ItemHolder Get<T> () {
			foreach (ItemHolder holder in holders) {
				if (holder is T)
					return holder;
			}
			Debug.LogError (string.Format ("ItemHolder {0} does not exist", typeof (T)));
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
			T sender = boundInventory.Get<T> () as T;
			T receiver = Get<T> () as T;
			receiver.Transfer (sender, amount, transferable);
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