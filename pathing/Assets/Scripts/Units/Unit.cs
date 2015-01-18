using UnityEngine;
using System.Collections;
using GameInput;

public class Unit : MonoBehaviour, IClickable, ISelectable {

	public UnitColorHandler colorHandler = new UnitColorHandler ();

	protected virtual void Awake () {
		colorHandler.Init (renderer);
	}

	public virtual void Click (ClickSettings settings) {
		
	}

	public virtual void OnSelect () {
		colorHandler.Selected = true;
	}

	public virtual void OnUnselect () {
		colorHandler.Selected = false;
	}
}
