using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DNA.Paths.Dijkstra;
using Delaunay;
using Delaunay.Geo;

namespace DNA.Paths {

	public static class Pathfinder {

		public static List<GridPoint> GetShortestPath (GridPoint a, GridPoint b) {
			
			var path = Engine.CalculateShortestPathBetween<GridPoint> (a, b, TreeGrid.Paths);
			List<GridPoint> pathList = new List<GridPoint> ();

			foreach (Path<GridPoint> gp in path) {
				pathList.Add (gp.Source);
			}

			//pathList.Add (path.Last.Value.Destination);

			return pathList;
		}
	}
}