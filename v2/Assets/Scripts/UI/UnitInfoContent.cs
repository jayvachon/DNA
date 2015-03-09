using UnityEngine;
using System.Collections;
using GameInventory;

public class UnitInfoContent {

	string title;
	public string Title {
		get { return title; }
	}

	Inventory inventory;
	public Inventory Inventory {
		get { return inventory; }
	}

	public UnitInfoContent (string title, Inventory inventory) {
		this.title = title;
		this.inventory = inventory;
	}
}
