using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class InventoryItemHolder : System.Object {}

[System.Serializable]
public class InventoryItemHolder<T> : InventoryItemHolder where T : InventoryItem {
	
	List<T> items = new List<T> ();
	
	public int Count {
		get { return items.Count; }
	}

	int maxCapacity;
	public int MaxCapacity {
		get { return maxCapacity; }
		set { maxCapacity = value; }
	}

	public InventoryItemHolder (int maxCapacity=1) {
		this.maxCapacity = maxCapacity;
	}

	public bool Add (List<T> t) {
		if (Count+t.Count > MaxCapacity) return false;
		foreach (T item in t) {
			items.Add (item);
		}
		return true;
	}

	public List<T> AddToMax (List<T> t) {
		int requestAmount = Count+t.Count;
		if (requestAmount <= MaxCapacity) {
			Add (t);
			return new List<T> (0);
		} else {
			int over = requestAmount-MaxCapacity;
			for (int i = 0; i < requestAmount-over; i ++) {
				items.Add (t[i]);
			}
			List<T> temp = new List<T> ();
			for (int i = requestAmount-over; i < t.Count; i ++) {
				temp.Add (t[i]);
			}
			return temp;
		}
	}

	public bool Subtract (int amount) {
		if (Count < amount) return false;
		for (int i = 0; i < amount; i ++) {
			items.RemoveAt (0);
		}
		return true;
	}

	public List<T> SubtractToMin (int amount) {
		if (Count <= amount) {
			return Clear ();
		} else {
			List<T> temp = new List<T> ();
			for (int i = 0; i < amount; i ++) {
				temp.Add (items[i]);
			}
			Subtract (amount);
			return temp;
		}
	}

	public List<T> Clear () {
		List<T> temp = new List<T> ();
		foreach (T t in items) {
			temp.Add (t);
		}
		items.Clear ();
		return temp;
	}
}
