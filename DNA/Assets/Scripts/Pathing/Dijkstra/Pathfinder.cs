using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DNA.Paths.Dijkstra;
using Delaunay;
using Delaunay.Geo;

namespace DNA.Paths {

	public static class Pathfinder {

		static Path<Vector2>[] path;
		static Path<Vector2>[] Path {
			get { 
				
				if (path == null) {
					
					List<LineSegment> connections = TreeGrid.Connections;
					path = new Path<Vector2>[connections.Count];

					for (int i = 0; i < path.Length; i ++) {

						List<Vector2> points = connections[i].Points;
						
						path[i] = new Path<Vector2> () {
							Source = points[0],
							Destination = points[1],
							Cost = (int)Vector2.Distance (points[0], points[1])
						};
					}
				}

				return path;
			}
		}

		public static void GetShortestPath (Vector3 a, Vector3 b) {

		}

		/*public static List<Path<Vector2>> GetShortestPath (Vector2 a, Vector2 b) {
			return new List<Path<Vector2>> (Engine.CalculateShortestPathBetween<Vector2> (a, b, Path));
		}*/

		// TODO: test this
		// will need to replace Vector2 with (new version of) PathPoint
		public static List<Vector2> GetShortestPath (Vector2 a, Vector2 b) {

			var paths = Engine.CalculateShortestPathBetween<Vector2> (a, b, Path);
			List<Vector2> points = new List<Vector2> ();

			foreach (Path<Vector2> p in paths)
				points.Add (p.Source);

			points.Add (paths.Last.Value.Destination);

			return points;
		}
	}
}