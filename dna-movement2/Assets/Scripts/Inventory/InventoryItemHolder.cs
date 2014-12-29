using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class InventoryItemHolder : System.Object {

	//TODO
	/*List<InventoryItem> items = new List<InventoryItem> ();
	int max = 5;

	public int Count {
		get { return items.Count; }
	}

	public bool HasItems {
		get { return Count > 0; }
	}

	public void Trade (InventoryItemHolder from) {
		from.Add (Add (from.Clear ()));
	}

	public int Add (int amount) {
		int overflow = amount + count - max;
		count = Mathf.Min (max, amount + count);
		return Mathf.Max (0, overflow);
	}

	public int Subtract (int amount) {
		int excess = amount - count;
		count = Mathf.Max (0, count - amount);
		return excess;
	}

	public int Clear () {
		int tempCount = count;
		count = 0;
		return tempCount;
	}*/
}
