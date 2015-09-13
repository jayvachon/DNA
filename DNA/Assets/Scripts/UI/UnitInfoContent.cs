using UnityEngine;
using System.Collections;
using DNA.InventorySystem;
using DNA.Units;
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
