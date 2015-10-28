using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DNA.InputSystem;
using DNA.Units;

public class GuiDescription : GuiSelectableListener {

	string name;
	string description;

	void Awake () {
		Init ();
	}

	protected override void OnUpdateSelection (List<ISelectable> selected) {

		if (selected.Count == 0) {
			name = "";
			description = "";
			return;
		}

		List<Unit> units = selected
			.FindAll (x => x is Unit)
			.ConvertAll (x => x as Unit);

		Unit commonUnit = units[0];

		foreach (Unit u in units) {
			if (u.Name != commonUnit.Name) {
				commonUnit = null;
				break;
			}
		}

		if (commonUnit == null) {
			name = "";
			description = "";
			return;
		}

		// TODO: display in gui
		name = commonUnit.Name + ((units.Count > 1) ? "s" : "");
		description = commonUnit.Description;

		if (commonUnit != null) {
			Debug.Log (name);
			Debug.Log (description);
		}
	}
}
