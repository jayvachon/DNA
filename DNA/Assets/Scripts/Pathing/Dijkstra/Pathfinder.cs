using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DNA.Paths.Dijkstra;
using Delaunay;
using Delaunay.Geo;

namespace DNA.Paths {

	public static class Pathfinder {

		static PathElement version = new PathElement ();

		static Path<GridPoint>[] paths;
		static Path<GridPoint>[] Paths {
			get {
				if (paths == null || !version.UpToDate) {

					List<Connection> connections = TreeGrid.Connections;
					paths = new Path<GridPoint>[connections.Count*2];

					int j = 0;
					for (int i = 0; i < paths.Length; i += 2) {
						paths[i] = connections[j].Path2Way[0];
						paths[i+1] = connections[j++].Path2Way[1];
					}

					version.SetUpToDate ();
				}
				return paths;
			}
		}

		public static List<GridPoint> GetShortestPath (GridPoint a, GridPoint b) {
			
			var path = Engine.CalculateShortestPathBetween<GridPoint> (a, b, Paths);
			List<GridPoint> pathList = new List<GridPoint> ();

			foreach (Path<GridPoint> gp in path) {
				pathList.Add (gp.Source);
			}

			pathList.Add (path.Last.Value.Destination);

			return pathList;
		}
	}
}