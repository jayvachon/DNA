using UnityEngine;
using System.Collections;

public class InventoryItem : System.Object {

	string name;
	public string Name {
		get { return name; }
	}

	int amount;
	public int Amount {
		get { return amount; }
	}
	int capacity;
	public int Capacity {
		get { return capacity; }
	}
	
	public InventoryItem (string name, int amount, int capacity) {
		this.name = name;
		this.amount = amount;
		this.capacity = capacity;
	}

	public bool Add (int a) {
		if (amount + a <= capacity) {
			amount += a;
			return true;
		}
		return false;
	}
	
	public bool Subtract (int a) {
		if (amount >= a) {
			amount -= a;
			return true;
		}
		return false;
	}

	public void Empty () {
		amount = 0;
	}
}
