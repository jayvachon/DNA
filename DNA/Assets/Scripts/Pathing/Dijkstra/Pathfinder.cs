using UnityEngine;
using System;
using System.Linq;
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
				//if (paths == null || !version.UpToDate) {

					List<Connection> connections = TreeGrid.Connections;
					paths = new Path<GridPoint>[connections.Count*2];

					int j = 0;
					for (int i = 0; i < paths.Length; i += 2) {
						paths[i] = connections[j].Path[0];
						paths[i+1] = connections[j++].Path[1];
					}

					version.SetUpToDate ();
				//}
				return paths;
			}
		}

		static Path<GridPoint>[] pathsNoFree;
		static Path<GridPoint>[] PathsNoFree {
			get {

				List<Connection> connections = TreeGrid.Connections.FindAll (x => x.Cost > 0);
				pathsNoFree = new Path<GridPoint>[connections.Count*2];

				int j = 0;

				for (int i = 0; i < pathsNoFree.Length; i += 2) {

					Connection c = connections[j++];

					pathsNoFree[i] = c.Path[0];
					pathsNoFree[i+1] = c.Path[1];
				}

				return pathsNoFree;
			}
		}

		static Path<GridPoint>[] pathsIgnoreCost;
		static Path<GridPoint>[] PathsIgnoreCost {
			get {

				List<Connection> connections = TreeGrid.Connections;
				pathsIgnoreCost = new Path<GridPoint>[connections.Count*2];

				int j = 0;
				for (int i = 0; i < pathsIgnoreCost.Length; i += 2) {

					Connection c = connections[j++];

					pathsIgnoreCost[i] = c.Path[0];
					pathsIgnoreCost[i].Cost = c.Costs["default"];
					pathsIgnoreCost[i+1] = c.Path[1];
					pathsIgnoreCost[i+1].Cost = c.Costs["default"];
				}

				return pathsIgnoreCost;
			}
		}

		static Path<GridPoint>[] freePaths;
		static Path<GridPoint>[] FreePaths {
			get {

				List<Connection> connections = TreeGrid.Connections.FindAll (x => x.Cost == 0);
				freePaths = new Path<GridPoint>[connections.Count*2];

				int j = 0;
				for (int i = 0; i < freePaths.Length; i += 2) {
					
					Connection c = connections[j++];

					freePaths[i] = c.Path[0];
					freePaths[i+1] = c.Path[1];
				}

				return freePaths;
			}
		}

		static Path<GridPoint>[] clearPaths;
		static Path<GridPoint>[] ClearPaths {
			get {

				List<Connection> connections = TreeGrid.Connections.FindAll (
					x => x.Cost > 0
					&& (!x.Path[0].Source.HasFog 
					|| !x.Path[0].Destination.HasFog));

				clearPaths = new Path<GridPoint>[connections.Count*2];

				int j = 0;
				for (int i = 0; i < clearPaths.Length; i += 2) {
					
					Connection c = connections[j++];

					clearPaths[i] = c.Path[0];
					clearPaths[i+1] = c.Path[1];
				}

				return clearPaths;
			}
		}

		public static List<GridPoint> ConnectedPoints {
			get { return TreeGrid.Points.FindAll (x => x.HasRoad); }
		}

		public static List<GridPoint> GetFreePath (GridPoint a, GridPoint b) {
			Path<GridPoint>[] free = FreePaths;
			return (PathsHavePoint (a, free) && PathsHavePoint (b, free)) 
				? GetPath (a, b, FreePaths)
				: new List<GridPoint> ();
		}

		public static List<GridPoint> GetCheapestPath (GridPoint a, GridPoint b) {
			return GetPath (a, b, Paths);
		}

		public static List<GridPoint> GetShortestPath (GridPoint a, GridPoint b) {
			return GetPath (a, b, PathsIgnoreCost);
		}

		public static List<GridPoint> GetPathNoOverlap (GridPoint a, GridPoint b) {
			//return GetPath (a, b, PathsNoFree);
			return GetPath (a, b, ClearPaths);
		}

		public static GridPoint FindNearestPoint (GridPoint a, Func<GridPoint, bool> requirement=null) {
			
			if (requirement == null)
				requirement = (GridPoint p) => { return true; };

			GridPoint nearest = null;
			int shortestPath = int.MaxValue;

			foreach (GridPoint b in ConnectedPoints) {
				if (b.Object != null && requirement (b)) {
					int pathLength = GetFreePath (a, b).Count;
					if (pathLength < shortestPath) {
						nearest = b;
						shortestPath = pathLength;
					}
				}
			}

			return nearest;
		}

		static List<GridPoint> GetPath (GridPoint a, GridPoint b, Path<GridPoint>[] pathToUse) {

			#if UNITY_EDITOR
			if (a == b)
				throw new System.Exception ("No path could be found because the two points are the same");
			#endif

			var path = Engine.CalculateShortestPathBetween<GridPoint> (a, b, pathToUse);
			List<GridPoint> pathList = new List<GridPoint> ();

			foreach (Path<GridPoint> gp in path)
				pathList.Add (gp.Source);

			pathList.Add (path.Last.Value.Destination);

			return pathList;
		}

		static bool PathsHavePoint (GridPoint p, Path<GridPoint>[] paths) {
			return System.Array.Find<Path<GridPoint>> (paths, x => x.Source == p || x.Destination == p) != null;
 		}

		public static List<Connection> PointsToConnections (List<GridPoint> points) {

			List<Connection> all = TreeGrid.Connections;
			List<Connection> connections = new List<Connection> ();

			for (int i = 0; i < points.Count-1; i ++) {
				if (points[i] != points[i+1])
					connections.Add (all.Find (x => x.ContainsPoints (points[i], points[i+1])));
			}

			return connections;
		}
	}
}