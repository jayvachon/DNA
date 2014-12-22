using UnityEngine;
using System.Collections;

[RequireComponent (typeof (LineRenderer))]
public class MovableUnit : Unit {

	Path path = new Path ();
	public Path MyPath {
		get { return path; }
	}

	PathDrawer pathDrawer;
	public PathDrawer MyPathDrawer {
		get { return pathDrawer; }
	}

	bool moving = false;
	float speed = 10f;

	public override void OnAwake () {
		base.OnAwake ();
		pathDrawer = new PathDrawer (
			path, 
			gameObject.GetComponent<LineRenderer>(), 
			colorHandler.DefaultColor
		);
	}

	/**
	 *	Private functions
	 */

	void StartMoveOnPath () {
		if (moving) return;
		Vector3[] line = path.GotoPosition ();
		if (line != null) {
			StartCoroutine (MoveOnPath (line));
		}
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

	/**
	 *	Debugging
	 */

	void Update () {
		if (!Selected) return;
		if (Input.GetKeyDown (KeyCode.Space)) {
			StartMoveOnPath ();
		}
	}
}
