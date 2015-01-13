using UnityEngine;
using System.Collections;
using GameActions;
using GameInput;

public class Unit : MBRefs, IClickable, ISelectable, IPoolable {

	public UnitColorHandler colorHandler = new UnitColorHandler ();

	protected override void Awake () {
		base.Awake ();
		colorHandler.Init (renderer);
	}

	public virtual void LeftClick () {
		SelectionManager.ToggleSelect (this);
	}

	public virtual void RightClick () {
		SelectionManager.Unselect ();
	}

	public virtual void OnSelect () {
		colorHandler.Selected = true;
	}

	public virtual void OnUnselect () {
		colorHandler.Selected = false;
	}
	
	// Poolable
	public virtual void OnCreate () {
		startPosition = MyTransform.position;
	}

	public virtual void OnDestroy () {}
}
