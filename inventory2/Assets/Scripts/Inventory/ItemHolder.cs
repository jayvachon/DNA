using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace GameInventory {

	public abstract class ItemHolder {
		
		protected List<Item> items;
		public List<Item> Items {
			get { return items; }
		}

		int capacity;
		public int Capacity {
			get { return capacity; }
			set { capacity = value; }
		}

		public int Count {
			get { return items.Count; }
		}

		public List<Item> EmptyList {
			get { return new List<Item> (0); }
		}
		
		public virtual List<Item> Add (Item item) { return EmptyList; }
		public virtual List<Item> Add (List<Item> newItems) { return EmptyList; }
		public virtual List<Item> Remove () { return EmptyList; }
		public virtual List<Item> Remove (int amount) { return EmptyList; }
		public virtual void Transfer (ItemHolder holder, int amount) {}
		public virtual void Print () {}
	}

	public class ItemHolder<T> : ItemHolder where T : Item {
		
		new protected List<T> items = new List<T> ();
		new public List<T> Items {
			get { return items; }
		}

		new public int Count {
			get { return items.Count; }
		}

		public ItemHolder (int capacity=1) {
			Capacity = capacity;
		}

		public override List<Item> Add (Item item) {
			List<Item> temp = new List<Item> ();
			temp.Add (item);
			return Add (temp);
		}

		public override List<Item> Add (List<Item> newItems) {
			while (Count < Capacity && newItems.Count > 0) {
				items.Add (newItems[0] as T);
				newItems.RemoveAt (0);
			}
			if (newItems.Count > 0) {
				return newItems; // returns items that couldn't be added
			} else {
				return EmptyList;
			}
		}

		public override List<Item> Remove () {
			return Remove (1);
		}

		public override List<Item> Remove (int amount) {
			List<Item> temp = new List<Item> (0);
			while (Count > 0 && amount > 0) {
				temp.Add (items[0]);
				items.RemoveAt (0);
				amount --;
			}
			return temp; // returns items that were removed
		}

		public override void Transfer (ItemHolder holder, int amount) {
			if (holder is ItemHolder<T>) {
				ItemHolder<T> sender = holder as ItemHolder<T>;
				List<Item> items = sender.Remove (amount);
				List<Item> overflow = Add (items);
				sender.Add (overflow);
			}
		}

		protected List<Item> ToItemsList<Y> (List<Y> childItems) where Y : Item {
			List<Item> temp = new List<Item> ();
			foreach (Y item in childItems) {
				temp.Add (item as Item);
			}
			return temp;
		}

		/**
		 *	Debugging
		 */

		public override void Print () {
			foreach (T item in items) {
				Debug.Log (item);
			}
		}
	}
}