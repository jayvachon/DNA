using UnityEngine;
using System.Collections;

public class Unit : MBRefs {

	public Color defaultColor;
	public Color selectColor = Color.red;
	public UnitColorHandler colorHandler = new UnitColorHandler ();

	bool selected = false;
	public bool Selected {
		get { return selected; }
	}

	bool selectable = true;
	public bool Selectable {
		get { return selectable; }
		set { selectable = value; }
	}

	public override void OnAwake () {
		Events.instance.AddListener<FloorClickEvent>(OnFloorClickEvent);
		Events.instance.AddListener<UnitClickEvent>(OnUnitClickEvent);
		colorHandler.OnStart (defaultColor, selectColor, this);
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
		//renderer.SetColor (selectColor);
		colorHandler.OnSelect ();
	}

	public virtual void OnUnselect () {
		//renderer.SetColor (defaultColor);
		colorHandler.OnUnselect ();
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

public class UnitColorHandler {

	Color defaultColor;
	Color selectColor;
	Renderer renderer;
	bool activated = false;

	public Color DefaultColor {
		get { return defaultColor; }
	}

	public Color SelectColor {
		get { return selectColor; }
	}

	public void OnStart (Color defaultColor, Color selectColor, Unit unit) {
		if (unit.renderer) {
			this.renderer = unit.renderer;
			this.defaultColor = defaultColor;
			this.selectColor = selectColor;
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
