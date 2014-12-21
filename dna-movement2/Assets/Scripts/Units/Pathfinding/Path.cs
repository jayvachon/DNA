using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Path {

	List<PathPoint> points = new List<PathPoint>();

	PathPoint prevPoint = null;
	PathPoint currPoint = null;
	int currPointIndex = 0;

	/**
	*	Properties
	*/

	public bool IsLoop {
		get { return FirstPoint == LastPoint; }
	}

	public Vector3 PrevPosition {
		get { return prevPoint.Position; }
	}

	public Vector3 CurrPosition {
		get { return currPoint.Position; }
	}

	public PathPoint FirstPoint {
		get { return points[0]; }
	}

	public PathPoint LastPoint {
		get { return points[points.Count-1]; }
	}

	/**
	*	Public functions
	*/

	/*public void ArriveAtPoint (MovableUnit u) {
		currPoint.ArriveAtPoint (u);
	}*/

	public Vector3[] GotoPosition () {
		
		if (points.Count < 2)
			return null;

		if (currPoint == null)
			GotoStartPosition ();
		GotoNextPosition ();

		return new Vector3[] { PrevPosition, CurrPosition };
	}

	public void AddPoint (PathPoint point) {
		if (CanAddPoint (point)) {
			points.Add (point);
		}
	}

	public void RemovePoint (PathPoint point) {
		
		if (!CanRemovePoint (point))
			return;

		points.Remove (point);
		/*List<PathPoint> tempPoints = new List<PathPoint>();
		for (int i = 0; i < points.Count; i ++) {
			if (points[i] != point)
				tempPoints.Add (points[i]);
		}*/

		// Special case if there are three points in the new path-
		// if the points form a loop, remove the last point (destroy the loop)
		/*PathPoint lastPoint = tempPoints[tempPoints.Count-1];
		if (tempPoints.Count == 3 && tempPoints[0] == lastPoint) {
			tempPoints.Remove (lastPoint);
		}*/
		if (points.Count == 3 && IsLoop) {
			points.Remove (LastPoint);
		}

		//points = tempPoints;
	}

	public Vector3[] GetPositions () {
		Vector3[] positions = new Vector3[points.Count];
		for (int i = 0; i < points.Count; i ++) {
			positions[i] = points[i].Position;
		}
		return positions;
	}

	/**
	*	Private functions
	*/

	Vector3 GotoStartPosition () {
		
		if (points.Count <= 0)
			return ExtensionMethods.NullPosition;

		currPoint = FirstPoint;
		currPointIndex = 0;
		return currPoint.Position;
	}

	Vector3 GotoNextPosition () {
		if (points.Count < 2)
			return ExtensionMethods.NullPosition;
		prevPoint = currPoint;
		SetNextPoint ();
		currPoint = points[currPointIndex];
		return currPoint.Position;
	}

	bool PathHasPoint (PathPoint point) {
		foreach (PathPoint p in points) {
			if (p == point)
				return true;
		}
		return false;
	}

	bool CanRemovePoint (PathPoint point) {
		return PathHasPoint (point) && !(point == prevPoint || point == currPoint);
	}

	bool CanAddPoint (PathPoint point) {
		if (!point.Activated)
			return false;
		if (points.Count == 0)
			return true;
		if (point == LastPoint)
			return false;
		return true;
	}

	void SetNextPoint () {
		
		// Go to the next point in the path
		// If it's the last point in the path:
		// a. loop if the last point is the same as the first point, otherwise,
		// b. reverse the direction of the path

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
		points.Reverse ();
		/*List<PathPoint> tempPoints = new List<PathPoint>();
		for (int i = points.Count-1; i > -1; i --) {
			tempPoints.Add (points[i]);
		}
		points = tempPoints;*/
	}

	/**
	*	Debugging
	*/

	public void Print () {
		Debug.Log ("==========");
		foreach (PathPoint p in points) {
			Debug.Log (p.Position);
		}
	}

	public void PrintFirstAndLast () {
		Debug.Log ("===========");
		Debug.Log (FirstPoint.Position);
		Debug.Log (LastPoint.Position);
	}
}
