using UnityEngine;
using System.Collections;

[RequireComponent (typeof (LineRenderer))]
public class MovableUnit : Unit, IPathable, IElderHoldable {

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

	public IceCreamHolder iceCreamHolder = new IceCreamHolder ();
	ElderHolder elders = new ElderHolder ();
	public ElderHolder Elders {
		get { return elders; }
	}

	public override void OnAwake () {
		base.OnAwake ();
		pathDrawer = new PathDrawer (
			MyPath, 
			gameObject.GetComponent<LineRenderer>(), 
			colorHandler.DefaultColor
		);
	}

	/**
	 *	Private functions
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
		ActionManager.instance.StartAction (path.CurrentPoint as StaticUnit, this);
	}

	public override void OnDepart () {
		StartMoveOnPath ();
	}

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

	void Update () {
		if (!canMove)
			return;
		if (Input.GetKeyDown (KeyCode.Space)) {
			StartMoveOnPath ();
		}
	}
}
