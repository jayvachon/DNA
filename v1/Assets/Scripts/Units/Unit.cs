using UnityEngine;
using System.Collections;
using GameInput;

public class Unit : MBRefs, INameable, IClickable, ISelectable, IPoolable {

	public UnitColorHandler colorHandler = new UnitColorHandler ();

	public virtual string Name {
		get { return ""; }
	}

	protected override void Awake () {
		base.Awake ();
		colorHandler.Init (renderer);
	}

	// IClickable
	public virtual void OnClick (ClickSettings clickSettings) {}

	// ISelectable
	public virtual void OnSelect () {
		colorHandler.Selected = true;
	}

	public virtual void OnUnselect () {
		colorHandler.Selected = false;
	}

	// IPoolable
	public virtual void OnCreate () {
		startPosition = MyTransform.position;
	}

	public virtual void OnDestroy () {}
}
