using UnityEngine;
using System.Collections;

public class ActionsList {

	public readonly Action[] actions;

	string[] actionNames = new string[0];
	public string[] ActionNames {
		get {
			if (actionNames.Length == 0) {
				actionNames = new string[actions.Length];
				for (int i = 0; i < actions.Length; i ++) {
					actionNames[i] = actions[i].name;
				}
			}
			return actionNames;
		}
	}

	// An empty ActionsList - will not display anything in the GUI
	public ActionsList () {
		actions = new Action[0];
	}

	// Custom ActionsList
	public ActionsList (Unit unit, Action[] actions) {
		this.actions = actions;
		for (int i = 0; i < actions.Length; i ++) {
			actions[i].SetUnit (unit);
		}
	}

	// An ActionsList built from a category
	public ActionsList (Unit unit, string categoryName) {
		
		UnitsCategory category = StaticUnitsHolder.instance.GetCategory (categoryName);
		string[] unitNames = category.UnitNames;
		
		actions = new Action[unitNames.Length];
		for (int i = 0; i < actions.Length; i ++) {
			actions[i] = new Action (unitNames[i], unit);
		}
	}

	public void Activate () {
		Events.instance.Raise (new ActivateActionsListEvent (this));
	}

	public void Deactivate () {
		Events.instance.Raise (new DeactivateActionsListEvent (this));
	}
}
