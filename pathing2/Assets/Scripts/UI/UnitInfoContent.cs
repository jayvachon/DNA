using UnityEngine;
using System.Collections;
using GameActions;
using GameInventory;
using Units;

public class UnitInfoContent {

	string title;
	public string Title {
		get { return title; }
	}

	Inventory inventory;
	public Inventory Inventory {
		get { return inventory; }
	}

	PerformableActions performableActions = null;

	/*public UnitInfoContent (string title, Inventory inventory, PerformableActions performable=null) {
		this.title = title;
		this.inventory = inventory;
	}*/

	public UnitInfoContent (Unit unit) {
		title = unit.Name;
		inventory = unit.Inventory;
		IActionPerformer actionPerformer = unit as IActionPerformer;
		if (actionPerformer != null) {
			performableActions = actionPerformer.PerformableActions;
		} else {
			performableActions = null;
		}
	}
}
