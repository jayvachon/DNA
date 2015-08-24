using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Delaunay;
using Delaunay.Geo;

public class VoronoiTest : MonoBehaviour {

	public Transform pointsParent;
	List<LineSegment> edges = null;
	float mapWidth = 50f;
	float mapHeight = 50f;

	List<Transform> pointTransforms;
	List<Transform> PointTransforms {
		get {
			if (pointTransforms == null && pointsParent != null) {
				pointTransforms = pointsParent.GetChildren ();
			}
			return pointTransforms;
		}
	}

	List<Vector3> Points {
		get { return pointTransforms.ConvertAll (p => p.position); }
	}

	List<Vector2> Points2 {
		get { return Points.ConvertAll (p => new Vector2 (p.x, p.z)); }
	}

	void Update () {
		List<uint> colors = new List<uint> ();
		for (int i = 0; i < PointTransforms.Count; i ++) {
			colors.Add (0);
		}
		Delaunay.Voronoi v = new Delaunay.Voronoi (Points2, colors, new Rect (-mapWidth*0.5f, -mapHeight*0.5f, mapWidth, mapHeight));
		edges = v.VoronoiDiagram ();
	}

	void OnDrawGizmos () {
		if (edges != null) {
			Gizmos.color = Color.yellow;
			for (int i = 0; i< edges.Count; i++) {
				Vector2 left = (Vector2)edges [i].p0;
				Vector2 right = (Vector2)edges [i].p1;
				Gizmos.DrawLine (new Vector3 (left.x, 0, left.y), new Vector3 (right.x, 0, right.y));
			}
		}

		Gizmos.color = Color.yellow;
		float l = -mapWidth*0.5f;
		float r = mapWidth*0.5f;
		float t = -mapHeight*0.5f;
		float b = mapHeight*0.5f;
		Gizmos.DrawLine (new Vector3 (l, 0, t), new Vector3 (r, 0, t));
		Gizmos.DrawLine (new Vector3 (l, 0, t), new Vector3 (l, 0, b));
		Gizmos.DrawLine (new Vector3 (l, 0, b), new Vector3 (r, 0, b));
		Gizmos.DrawLine (new Vector3 (r, 0, t), new Vector3 (r, 0, b));
	}
}
