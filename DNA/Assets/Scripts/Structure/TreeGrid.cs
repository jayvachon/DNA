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

		static Fermat fermat = new Fermat (new Fermat.Settings (1.25f, 200, 0.05f, new Vector3 (0, 6.5f, 0)));
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

		static List<GridPoint[]> connections;
		public static List<GridPoint[]> Connections {
			get {
				if (connections == null) {
					
					List<LineSegment> segments = Voronoi.DelaunayTriangulation ();
					connections = new List<GridPoint[]> ();

					foreach (LineSegment segment in segments) {

						List<Vector2> s = segment.Points;

						connections.Add (
							new [] {
								Points.Find (p => Mathf.Approximately (s[0].x, p.Position.x) 
									&& Mathf.Approximately (s[0].y, p.Position.z)),
								Points.Find (p => Mathf.Approximately (s[1].x, p.Position.x) 
									&& Mathf.Approximately (s[1].y, p.Position.z))
							}
						);
					}
				}
				return connections;
			}
		}

		static Path<GridPoint>[] paths;
		public static Path<GridPoint>[] Paths {
			get {
				if (paths == null) {
					
					paths = new Path<GridPoint>[Connections.Count];

					for (int i = 0; i < paths.Length; i ++) {

						GridPoint[] connection = Connections[i];
						
						paths[i] = new Path<GridPoint> () {
							Source = connection[0],
							Destination = connection[1],
							Cost = (int)Vector3.Distance (connection[0].Position, connection[1].Position)
						};
					}
				}
				return paths;
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