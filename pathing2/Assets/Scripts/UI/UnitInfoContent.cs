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
	public PerformableActions PerformableActions {
		get { return performableActions; }
	}

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
