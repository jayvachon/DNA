using UnityEngine;
using System.Collections;

public class Unit : MBRefs, IClickable, ISelectable, IEnableable, IPoolable, IActionable {

	new bool enabled = true;
	public bool Enabled {
		get { return enabled; }
		set {
			enabled = value;
			if (enabled)
				OnEnable ();
			else
				OnDisable ();
		}
	}

	public UnitColorHandler colorHandler = new UnitColorHandler ();

	Unit selectedUnit = null;
	public Unit SelectedUnit {
		get { return selectedUnit; }
	}

	bool selected = false;
	public bool Selected {
		get { return selected; }
	}

	bool selectable = true;
	public bool Selectable {
		get { return selectable; }
		set { selectable = value; }
	}

	protected ActionsList actionsList = new ActionsList ();
	public ActionsList MyActionsList {
		get { return actionsList; }
	}

	public override void OnAwake () {
		Events.instance.AddListener<SelectUnitEvent> (OnSelectUnitEvent);
		Events.instance.AddListener<UnselectUnitEvent> (OnUnselectUnitEvent);
		Events.instance.AddListener<NullClickEvent> (OnNullClickEvent);
		colorHandler.Init (renderer);
	}

	public virtual void RightClick () {
		if (Enabled) Unselect ();
	}

	public virtual void LeftClick () {
		if (Enabled) ToggleSelect ();
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
		Events.instance.Raise (new SelectUnitEvent (this));
		OnSelect ();
	}

	public void Unselect () {
		if (!selectable || !selected) return;
		if (selectedUnit == this) {
			Events.instance.Raise (new UnselectUnitEvent ());
		}
		selected = false;
		OnUnselect ();
	}

	protected virtual void OnSelect () {
		colorHandler.Selected = true;
	}

	protected virtual void OnUnselect () {
		colorHandler.Selected = false;
	}

	void OnSelectUnitEvent (SelectUnitEvent e) {
		selectedUnit = e.unit;
	}

	void OnUnselectUnitEvent (UnselectUnitEvent e) {
		selectedUnit = null;
	}

	protected virtual void OnNullClickEvent (NullClickEvent e) {
		Unselect ();
	}

	// Actionable
	public virtual void OnArrive () {}
	public virtual void OnPerform (float progress) {}
	public virtual void OnDepart () {}

	// Enableable
	public virtual void OnEnable () {}
	public virtual void OnDisable () {}
	
	// Poolable
	public virtual void OnCreate () {
		startPosition = MyTransform.position;
	}

	public virtual void OnDestroy () {}
}
