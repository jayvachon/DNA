using UnityEngine;
using System.Collections;

public class Unit : MBRefs, IClickable, ISelectable {

	public static Unit selectedUnit = null;

	bool selected = false;
	public bool Selected {
		get { return selected; }
	}

	bool selectable = true;
	public bool Selectable {
		get { return selectable; }
		set { selectable = value; }
	}

	public void RightClick () {
		Unselect ();
	}

	public void LeftClick () {
		ToggleSelect ();
	}

	public void ToggleSelect () {
		if (selected) 
			Unselect ();
		else 
			Select ();
	}

	public void Select () {
		if (!selectable || selected) return;
		if (selectedUnit != null) {
			selectedUnit.Unselect ();
		}
		selected = true;
		selectedUnit = this;
		OnSelect ();
	}

	public void Unselect () {
		if (!selectable || !selected) return;
		if (selectedUnit == this) {
			selectedUnit = null;
		}
		selected = false;
		OnUnselect ();
	}

	protected virtual void OnSelect () {}
	protected virtual void OnUnselect () {}
}
