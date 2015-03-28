using UnityEngine;
using System.Collections;
using GameActions;
using GameInventory;
using Units;

public delegate void ContentUpdated ();

public class UnitInfoContent {

	public ContentUpdated contentUpdated;

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

	Unit unit = null;

	public UnitInfoContent (Unit unit) {
		this.unit = unit;
		Set ();
	}

	void Set () {
		title = unit.Name;
		inventory = unit.Inventory;
		IActionPerformer actionPerformer = unit as IActionPerformer;
		if (actionPerformer != null) {
			performableActions = actionPerformer.PerformableActions;
		} else {
			performableActions = null;
		}
		if (contentUpdated != null) {
			contentUpdated ();
		}
	}

	public void Refresh () {
		Set ();
	}
}
