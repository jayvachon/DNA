using UnityEngine;
using System.Collections;

public class Unit : MBRefs {

	// Colors the unit based on whether it's selected or not
	public UnitColorHandler colorHandler = new UnitColorHandler ();

	// The actions that the unit can perform (displayed in the GUI)
	protected ActionsList actionsList = new ActionsList ();

	// Whether or not the unit is selected
	bool selected = false;
	public bool Selected {
		get { return selected; }
	}

	// Whether or not the unit can be selected
	bool selectable = true;
	public bool Selectable {
		get { return selectable; }
		set { selectable = value; }
	}

	public override void OnAwake () {
		Events.instance.AddListener<FloorClickEvent>(OnFloorClickEvent);
		Events.instance.AddListener<UnitClickEvent>(OnUnitClickEvent);
		colorHandler.OnStart (this);
	}

	/**
	*	Public functions
	*/

	public void Select () {
		if (!selectable || selected) return;
		selected = true;
		OnSelect ();
	}

	public void Unselect () {
		if (!selectable || !selected) return;
		selected = false;
		OnUnselect ();
	}

	public void ToggleSelect () {
		if (selected) 
			Unselect ();
		else 
			Select ();
	}

	/**
	*	Virtual functions
	*/

	public virtual void OnSelect () {
		colorHandler.OnSelect ();
		actionsList.Activate ();
	}

	public virtual void OnUnselect () {
		colorHandler.OnUnselect ();
		actionsList.Deactivate ();
	}

	public virtual void ClickThis () {
		ToggleSelect (); 
	}

	public virtual void ClickOther (UnitClickEvent e) {
		Unselect ();
	}

	public virtual void ClickNothing () {
		Unselect ();
	}

	public virtual void OnPerformAction (Action action) {}

	/**
	*	Messages
	*/

	public virtual void OnUnitClickEvent (UnitClickEvent e) {
		if (e.transform == MyTransform)
			ClickThis ();
		else
			ClickOther (e);
	}

	public virtual void OnFloorClickEvent (FloorClickEvent e) {
		ClickNothing ();	
	}
}

[System.Serializable]
public class UnitColorHandler : System.Object {

	public Color defaultColor = Color.white;
	public Color selectColor = Color.red;
	Renderer renderer;
	bool activated = false;

	public Color DefaultColor {
		get { return defaultColor; }
	}

	public Color SelectColor {
		get { return selectColor; }
	}

	public void OnStart (Unit unit) {
		if (unit.renderer) {
			this.renderer = unit.renderer;
			activated = true;
			renderer.SetColor (defaultColor);
		}
	}

	public void OnSelect () {
		if (!activated) return;
		renderer.SetColor (selectColor);
	}

	public void OnUnselect () {
		if (!activated) return;
		renderer.SetColor (defaultColor);
	}
}
