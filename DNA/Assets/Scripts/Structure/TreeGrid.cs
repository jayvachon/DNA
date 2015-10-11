using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Delaunay;
using Delaunay.Geo;
using DNA.Paths;
using DNA.Paths.Dijkstra;

namespace DNA {

	public static class TreeGrid {

		// Private fields
		
		static Fermat fermat = new Fermat (new Fermat.Settings (1.5f, 200, 0.05f, new Vector3 (0, 6.5f, 0)));
		static float mapWidth = 200f;
		static float mapHeight = 200f;

		// Public properties

		static List<GridPoint> points;
		public static List<GridPoint> Points {
			get {
				if (points == null) {
					points = new List<GridPoint> ();
					foreach (Vector3 v in fermat.Points)
						points.Add (new GridPoint { Position = v });
				}
				return points;
			}
		}

		static List<Connection> connections;
		public static List<Connection> Connections {
			get {
				if (connections == null) {
					
					List<LineSegment> segments = Voronoi.DelaunayTriangulation ();
					connections = new List<Connection> ();

					foreach (LineSegment segment in segments) {

						List<Vector2> s = segment.Points;
						
						GridPoint p1 = Points.Find (p => Mathf.Approximately (s[0].x, p.Position.x) 
							&& Mathf.Approximately (s[0].y, p.Position.z));
						GridPoint p2 = Points.Find (p => Mathf.Approximately (s[1].x, p.Position.x) 
							&& Mathf.Approximately (s[1].y, p.Position.z));

						Connection c = new Connection (new [] { p1, p2 });
						
						p1.Connections.Add (c);
						p2.Connections.Add (c);
						connections.Add (c);
					}
				}
				return connections;
			}
		}

		// Private properties

		static Delaunay.Voronoi voronoi;
		static Delaunay.Voronoi Voronoi {
			get {
				if (voronoi == null) {
					
					// TODO: don't require colors in the voronoi constructor
					List<uint> colors = new List<uint> ();
					foreach (GridPoint point in Points)
						colors.Add (0);

					voronoi = new Delaunay.Voronoi (
						Points.ConvertAll (x => new Vector2 (x.Position.x, x.Position.z)), 
						colors, 
						new Rect (-mapWidth*0.5f, -mapHeight*0.5f, mapWidth, mapHeight)
					);
				}
				return voronoi;
			}
		}
	}
}