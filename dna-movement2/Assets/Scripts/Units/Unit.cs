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

	protected ActionsList actionsList = new ActionsList ();
	public ActionsList MyActionsList {
		get { return actionsList; }
	}

	public override void OnAwake () {
		colorHandler.Init (renderer);
	}

	public virtual void LeftClick () {
		if (Enabled) SelectionManager.ToggleSelect (this);
	}

	public virtual void RightClick () {
		if (Enabled) SelectionManager.Unselect ();
	}

	public virtual void OnSelect () {
		colorHandler.Selected = true;
		ShowActions ();
	}

	public virtual void OnUnselect () {
		colorHandler.Selected = false;
		HideActions ();
	}

	// Actionable
	public void ShowActions () {
		ActionsListManager.Actions = MyActionsList;
	}

	public void HideActions () {
		ActionsListManager.Actions = null;
	}

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

	public virtual void OnDestroy () {
		
	}
}
