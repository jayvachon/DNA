using UnityEngine;
using System.Collections;
using Pathing;
using GameActions;
using GameInput;

public class MovableUnit : Unit, IPathable, IBinder {

	public Path Path { get; set; }
	public IActionAcceptor BoundAcceptor { get; private set; }

	protected override void Awake () {
		base.Awake ();
		Path = Path.Create (this, uTransform);
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

	public virtual void ArriveAtPoint (IPathPoint point) {
		if (point is IActionAcceptor) {
			OnBindActionable (point as IActionAcceptor);
		} else {
			StartMoveOnPath ();
		}
	}

	protected virtual void OnBindActionable (IActionAcceptor acceptor) {
		BoundAcceptor = acceptor;
		ActionHandler.instance.Bind (this);
	}

	public void OnEndActions () {
		StartMoveOnPath ();
	}

	// Debugging
	void Update () {
		if (!Path.Enabled) return;
		if (Input.GetKeyDown (KeyCode.Space)) {
			StartMoveOnPath ();
		}
	}
}
