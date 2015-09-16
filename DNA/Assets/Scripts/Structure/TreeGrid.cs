using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Delaunay;
using Delaunay.Geo;

public static class TreeGrid {

	static Fermat fermat = new Fermat (new Fermat.Settings (1.25f, 200, 0.05f, new Vector3 (0, 6.5f, 0)));
	static float mapWidth = 200f;
	static float mapHeight = 200f;

	public static Vector3[] PointsArr {
		get { return fermat.Points; }
	}
	
	public static int PointCount {
		get { return PointsArr.Length; }
	}


	static List<Vector3> points;
	public static List<Vector3> Points {
		get {
			if (points == null) {
				points = new List<Vector3> (PointsArr);
			}
			return points;
		}
	}

	static List<LineSegment> connections;
	public static List<LineSegment> Connections {
		get {
			if (connections == null) {
				connections = Voronoi.DelaunayTriangulation ();
			}
			return connections;
		}
	}

	static Delaunay.Voronoi voronoi;
	static Delaunay.Voronoi Voronoi {
		get {
			if (voronoi == null) {
				
				// TODO: don't require this in the voronoi constructor
				List<uint> colors = new List<uint> ();
				foreach (Vector3 point in Points)
					colors.Add (0);

				voronoi = new Delaunay.Voronoi (
					Points.ConvertAll (x => new Vector2 (x.x, x.z)), colors, new Rect (-mapWidth*0.5f, -mapHeight*0.5f, mapWidth, mapHeight));
			}
			return voronoi;
		}
	}

	public static List<Vector3> EdgeToV3 (LineSegment edge) {
		Vector2 e0 = (Vector2)edge.p0;
		Vector2 e1 = (Vector2)edge.p1;
		return new List<Vector3> () {
			Points.Find (x => Mathf.Approximately (x.x, e0.x) && Mathf.Approximately (x.z, e0.y)),
			Points.Find (x => Mathf.Approximately (x.x, e1.x) && Mathf.Approximately (x.z, e1.y))
		};
	}
}
