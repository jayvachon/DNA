using UnityEngine;
using System.Collections;
using Pathing;
using GameInput;

public class MovableUnit : Unit, IPathable {

	public Path Path { get; set; }

	protected override void Awake () {
		base.Awake ();
		Path = Path.Create (this);
	}

	public override void OnClick (ClickSettings clickSettings) {
		if (clickSettings.left) {
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

	public void StartMoveOnPath () {
		Path.Move ();
	}

	public void ArriveAtPoint (IPathPoint pathPoint) {
		StartMoveOnPath ();
	}

	void Update () {
		if (Input.GetKeyDown (KeyCode.Space)) {
			StartMoveOnPath ();
		}
	}
}
