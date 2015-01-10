using UnityEngine;
using System.Collections;

[System.Serializable]
public class InventoryItem : System.Object {

	public readonly string name;

	int count = 0;
	public int Count {
		get { return count; }
	}

	int maxCapacity;
	public int MaxCapacity {
		get { return maxCapacity; }
		set { maxCapacity = value; }
	}

	public InventoryItem (string name, int maxCapacity=1, int initialCount=0) {
		this.name = name;
		this.maxCapacity = maxCapacity;
		count = initialCount;
	}

	public bool Add (int amount) {
		if (count + amount <= maxCapacity) {
			count += amount;
			return true;
		}
		return false;
	}

	public int GetAdd (int amount) {
		int requestAmount = count + amount;
		if (requestAmount <= maxCapacity) {
			count = requestAmount;
			return 0;
		} else {
			count = maxCapacity;
			return requestAmount - maxCapacity;
		}
	}

	public bool Subtract (int amount) {
		if (count >= amount) {
			count -= amount;
			return true;
		}
		return false;
	}

	public int GetSubtract (int amount) {
		if (count >= amount) {
			count -= amount;
			return amount;
		} else {
			return Clear ();
		}
	}

	public int Clear () {
		int tempCount = count;
		count = 0;
		return tempCount;
	}
}
