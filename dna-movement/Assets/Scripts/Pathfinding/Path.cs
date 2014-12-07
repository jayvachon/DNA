using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Path {

	List<PathPointContainer> points = new List<PathPointContainer>();

	Vector3 nullPoint = new Vector3 (-1, -1, -1);

	PathPointContainer prevPoint = null;
	PathPointContainer currPoint = null;
	int currPointIndex = 0;

	/**
	*	Properties
	*/

	public bool IsLoop {
		get { return points[0] == points[points.Count-1]; }
	}

	public Vector3 PrevPosition {
		get { return prevPoint.StartPosition; }
	}

	public Vector3 CurrPosition {
		get { return currPoint.StartPosition; }
	}

	/**
	*	Public functions
	*/

	public void ArriveAtPoint (MovableUnit u) {
		currPoint.ArriveAtPoint (u);
	}

	public Vector3[] GotoPoint () {
		
		if (points.Count < 2)
			return null;

		if (currPoint == null)
			GotoStartPoint ();
		GotoNextPoint ();

		return new Vector3[] { PrevPosition, CurrPosition };
	}

	public void AddPoint (PathPointContainer point) {
		if (CanAddPoint (point)) {
			points.Add (point);
		}
	}

	public void RemovePoint (PathPointContainer point) {
		
		if (!CanRemovePoint (point))
			return;

		List<PathPointContainer> tempPoints = new List<PathPointContainer>();
		for (int i = 0; i < points.Count; i ++) {
			if (points[i] == point) {
				continue;
			}
			tempPoints.Add (points[i]);
		}

		// Special case if there are three points in the new path:
		// if the points form a loop, remove the last point (destroy the loop)
		PathPointContainer lastPoint = tempPoints[tempPoints.Count-1];
		if (tempPoints.Count == 3 && tempPoints[0] == lastPoint) {
			tempPoints.Remove (lastPoint);
		}

		points = tempPoints;
	}

	public Vector3[] GetPoints () {
		Vector3[] v3points = new Vector3[points.Count];
		for (int i = 0; i < points.Count; i ++) {
			v3points[i] = points[i].transform.position;
		}
		return v3points;
	}

	/**
	*	Private functions
	*/

	Vector3 GotoStartPoint () {
		if (points.Count > 0) {
			currPoint = points[0];
			currPointIndex = 0;
			return currPoint.StartPosition;
		}
		return nullPoint;
	}

	Vector3 GotoNextPoint () {
		if (points.Count < 2)
			return nullPoint;
		prevPoint = currPoint;
		SetNextPoint ();
		currPoint = points[currPointIndex];
		return currPoint.StartPosition;
	}

	bool PathHasPoint (PathPointContainer point) {
		foreach (PathPointContainer p in points) {
			if (p == point)
				return true;
		}
		return false;
	}

	bool CanRemovePoint (PathPointContainer point) {
		return PathHasPoint (point) && !(point == prevPoint || point == currPoint);
	}

	bool CanAddPoint (PathPointContainer point) {
		if (!point.Activated)
			return false;
		if (points.Count == 0)
			return true;
		if (points[points.Count-1] == point)
			return false;
		return true;
	}

	void SetNextPoint () {
		
		// Go to the next point in the path
		// If it's the last point in the path:
		// a. loop if the last point is the same as the first point,
		// b. otherwise, reverse the direction

		if (currPointIndex+1 > points.Count-1) {
			if (IsLoop) {
				currPointIndex = 1;
			} else {
				ReversePath ();
				currPointIndex = 1;
			}
		} else {
			currPointIndex ++;
		}
	}

	void ReversePath () {
		List<PathPointContainer> tempPoints = new List<PathPointContainer>();
		for (int i = points.Count-1; i > -1; i --) {
			tempPoints.Add (points[i]);
		}
		points = tempPoints;
	}

	/**
	*	Debugging
	*/

	public void Print () {
		Debug.Log ("==========");
		foreach (PathPointContainer p in points) {
			Debug.Log (p.transform.position);
		}
	}

	public void PrintFirstAndLast () {
		Debug.Log ("===========");
		Debug.Log (points[0].StartPosition);
		Debug.Log (points[points.Count-1].StartPosition);
	}
}
