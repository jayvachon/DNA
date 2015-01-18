using UnityEngine;
using System.Collections;
using Pathing;
using GameInput;

public class MovableUnit : Unit, IPathable {

	public Path Path { get; set; }

	protected override void Awake () {
		base.Awake ();
		Path = Path.Create (transform);
	}

	public override void Click (ClickSettings settings) {
		if (settings.Drag) return;
		if (settings.Left) {
			SelectionManager.ToggleSelect (this);
		} else {
			SelectionManager.Unselect ();
		}
	}

	public override void OnSelect () {
		base.OnSelect ();
		Path.Enabled = true;
	}

	public override void OnUnselect () {
		base.OnUnselect ();
		Path.Enabled = false;
	}
}
