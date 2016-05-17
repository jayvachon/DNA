using UnityEngine;
using System.Collections;
using DNA.Paths;

public class GridPointDrawer : MonoBehaviour {

	public PointsManager points;

	void OnDrawGizmos () {

		foreach (PointContainer point in points.Points) {
			Gizmos.color = (point.Point.HasFog) ? Color.black : Color.yellow;
			Gizmos.DrawSphere (point.transform.position, 1f);
		}
	}
}
