using UnityEngine;
using System.Collections;

[RequireComponent (typeof (LineRenderer))]
public class MovableUnit : Unit {

	Path path = new Path ();
	LineRenderer lineRenderer;

	bool moving = false;
	float speed = 10f;

	void Start () {
		lineRenderer = gameObject.GetComponent<LineRenderer>();
		lineRenderer.SetColor (colorHandler.DefaultColor);
		OnStart ();
	}

	/**
	*	Public functions
	*/

	public override void ClickThis () {
		base.ClickThis ();
	}

	public override void ClickOther (UnitClickEvent e) {
		if (Selected) {
			if (e.unit is MovableUnit) {
				Unselect ();
			} else if (e.unit is PathPointContainer) {
				PathPointContainer ppc = e.unit as PathPointContainer;
				if (!ppc.Activated) {
					Unselect ();
					return;
				}
				if (e.leftClick) {
					path.AddPoint (ppc);
					UpdateLineRenderer ();
				} else {
					path.RemovePoint (ppc);
					UpdateLineRenderer ();
				}
			}
		}
	}

	public void OnEndPathAction () {
		StartMoveOnPath ();
	}

	/**
	*	Private functions
	*/

	void UpdateLineRenderer () {
		lineRenderer.SetVertexPositions (path.GetPoints ());
	}

	void StartMoveOnPath () {
		if (moving) return;
		Vector3[] line = path.GotoPoint ();
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
		path.ArriveAtPoint (this);
	}

	/**
	*	Virtual functions
	*/

	public virtual void OnStart () {}

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