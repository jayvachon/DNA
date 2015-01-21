using UnityEngine;
using System.Collections;
using GameInput;

public class Unit : MonoBehaviour, IClickable, ISelectable {

	public UnitColorHandler colorHandler = new UnitColorHandler ();

	protected virtual void Awake () {
		colorHandler.Init (renderer);
	}

	public virtual void Click (bool left) {}
	public virtual void Drag (bool left, Vector3 mousePosition) {}
	public virtual void Release (bool left) {}

	public virtual void OnSelect () {
		colorHandler.Selected = true;
	}

	public virtual void OnUnselect () {
		colorHandler.Selected = false;
	}
}
