using UnityEngine;
using System.Collections;
using GameInput;

public class Unit : MBRefs, INameable, IClickable, ISelectable, IPoolable {

	public UnitColorHandler colorHandler = new UnitColorHandler ();
	public UnitTransform uTransform;
	public UnitRender uRender;
	public Transform UTransform {
		get { return uTransform.MyTransform; }
	}

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
		uRender.OnSelect ();
	}

	public virtual void OnUnselect () {
		colorHandler.Selected = false;
		uRender.OnUnselect ();
	}

	// IPoolable
	public virtual void OnCreate () {
		startPosition = MyTransform.position;
	}

	public virtual void OnDestroy () {}
}
