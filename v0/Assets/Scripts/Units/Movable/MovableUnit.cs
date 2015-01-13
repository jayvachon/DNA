using UnityEngine;
using System.Collections;
using Pathing;
using GameActions;

[RequireComponent (typeof (LineRenderer))]
public class MovableUnit : Unit, IPathable {

	Path path = null;
	public Path MyPath {
		get { 
			if (path == null) {
				path = new Path (this);
			}
			return path; 
		}
	}

	public bool ActivePath {
		set {
			if (value) {
				PathManager.ActivePath = MyPath;
			} else {
				PathManager.ActivePath = null;
			}
		}
	}

	PathDrawer pathDrawer;
	public PathDrawer MyPathDrawer {
		get { return pathDrawer; }
	}

	bool canMove = false;
	bool moving = false;
	float speed = 10f;

	protected override void Awake () {
		base.Awake ();
		InitPath ();
	}

	protected void InitPath () {
		pathDrawer = new PathDrawer (
			MyPath, 
			gameObject.GetComponent<LineRenderer>(), 
			colorHandler.DefaultColor
		);
	}

	/**
	 *	Public functions
	 */

	public void StartMoveOnPath () {
		if (moving) return;
		Vector3[] line = path.GotoPosition ();
		if (line != null) {
			StartCoroutine (MoveOnPath (line));
		}
	}

	public void OnUpdatePath () {
		pathDrawer.Update ();
	}

	IEnumerator MoveOnPath (Vector3[] line) {

		moving = true;

		Vector3 start = line[0];
		Vector3 end = line[1];

		float distance = Vector3.Distance (start, end);
		float time = distance / speed;
		float eTime = 0f;

		while (eTime < time) {
			eTime += Time.deltaTime;
			MyTransform.position = Vector3.Lerp (start, end, eTime / time);
			yield return null;
		}

		moving = false;
		OnArriveAtPoint ();
	}

	public void OnArriveAtPoint () {
		if (path.CurrentPoint is IActionAcceptor) {
			OnBindActionable (path.CurrentPoint as IActionAcceptor);
		}
	}

	protected virtual void OnBindActionable (IActionAcceptor acceptor) {}

	public override void OnSelect () {
		base.OnSelect ();
		ActivePath = true;
		canMove = true;
	}

	public override void OnUnselect () {
		base.OnUnselect ();
		ActivePath = false;
		canMove = false;
	}

	/**
	 *	Debugging
	 */

	protected virtual void Update () {
		if (!canMove)
			return;
		if (Input.GetKeyDown (KeyCode.Space)) {
			StartMoveOnPath ();
		}
	}
}
