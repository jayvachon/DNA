using UnityEngine;
using System.Collections;
using GameActions;
using GameInventory;
using Units;
using DNA.Tasks;

public delegate void ContentUpdated ();

public class UnitInfoContent {

	public ContentUpdated contentUpdated;

	string title;
	public string Title {
		get { return title; }
	}

	string description;
	public string Description {
		get { return description; }
	}

	Inventory inventory;
	public Inventory Inventory {
		get { return inventory; }
	}

	/*PerformableActions performableActions = null;
	public PerformableActions PerformableActions {
		get { return performableActions; }
	}*/

	public PerformableTasks PerformableTasks { get; private set; }

	Unit unit = null;

	public UnitInfoContent (Unit unit) {
		this.unit = unit;
		Set ();
	}

	void Set () {
		title = unit.Name;
		description = unit.Description;
		inventory = unit.Inventory;
		/*IActionPerformer actionPerformer = unit as IActionPerformer;
		if (actionPerformer != null) {
			performableActions = actionPerformer.PerformableActions;
		} else {
			performableActions = null;
		}*/
		ITaskPerformer performer = unit as ITaskPerformer;
		if (performer != null) {
			PerformableTasks = performer.PerformableTasks;
		} else {
			PerformableTasks = null;
		}
		if (contentUpdated != null) {
			contentUpdated ();
		}
	}

	public void Refresh () {
		Set ();
	}
}
