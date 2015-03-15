using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace GameInventory {

	public delegate bool ItemHasAttribute (Item item);
	public delegate void InventoryUpdated ();

	[System.Serializable]
	public class Inventory : System.Object {

		#if UNITY_EDITOR
		public string[] holderInfo;
		public int updateCount = 0;
		#endif

		public InventoryUpdated inventoryUpdated;

		List<ItemHolder> holders = new List<ItemHolder> ();
		public List<ItemHolder> Holders {
			get { return holders; }
		}

		public void Add (ItemHolder holder) {
			holders.Add (holder);
			#if UNITY_EDITOR
			holder.HolderUpdated += OnUpdateHolder;
			UpdateHolderInfo ();
			#endif
			NotifyInventoryUpdated ();
		}

		public List<Item> AddItem<T> (Item item) where T : ItemHolder {
			T holder = Get<T> () as T;
			NotifyInventoryUpdated ();
			return holder.Add (item);
		}

		public List<Item> AddItems<T> (List<Item> items) where T : ItemHolder {
			T holder = Get<T> () as T;
			NotifyInventoryUpdated ();
			return holder.Add (items);
		}

		public List<Item> RemoveItem<T> () where T : ItemHolder {
			T holder = Get<T> () as T;
			NotifyInventoryUpdated ();
			return holder.Remove ();
		}

		public List<Item> RemoveItems<T> (int amount) where T : ItemHolder {
			T holder = Get<T> () as T;
			NotifyInventoryUpdated ();
			return holder.Remove (amount);
		}

		public ItemHolder Get<T> () {
			foreach (ItemHolder holder in holders) {
				if (holder is T)
					return holder;
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
			T sender = boundInventory.Get<T> () as T;
			T receiver = Get<T> () as T;
			receiver.Transfer (sender, amount, transferable);
			NotifyInventoryUpdated ();
		}

		public void NotifyInventoryUpdated () {
			if (inventoryUpdated != null) {
				inventoryUpdated ();
			}
			#if UNITY_EDITOR
			UpdateHolderInfo ();
			#endif
		}

		void OnUpdateHolder () {
			NotifyInventoryUpdated ();
			#if UNITY_EDITOR
			UpdateHolderInfo ();
			#endif
		}

		/**
		 *	Debugging
		 */

		#if UNITY_EDITOR
		void UpdateHolderInfo () {
			holderInfo = new string[holders.Count];
			for (int i = 0; i < holders.Count; i ++) {
				ItemHolder holder = holders[i];
				holderInfo[i] = string.Format ("{0}: {1}/{2}", holder.Name, holder.Count, holder.Capacity);
			}
			updateCount ++;
		}
		#endif

		public virtual void Print () {
			foreach (ItemHolder holder in holders) {
				holder.Print ();
			}
		}
	}
}