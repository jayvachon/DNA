using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pathing;

namespace DNA.Paths {

	public class PathDebugger : MBRefs {

		static bool first = true;
		static List<GridPoint> points = new List<GridPoint> ();
		static List<GridPoint> path;

		LineDrawer line = null;
		LineDrawer Line {
			get {
				if (line == null) {
					line = MyTransform.GetChild (0).GetComponent<LineDrawer> ();
				}
				return line;
			}
		}

		static PathDebugger instance;
		static PathDebugger Instance {
			get { 
				if (instance == null) {
					instance = Object.FindObjectOfType (typeof (PathDebugger)) as PathDebugger;
				}
				return instance;
			}
		}

		public static void AddPoint (GridPoint p) {
			if (points.Contains (p))
				return;
			if (points.Count < 2) {
				points.Add (p);
			} else {
				points[first ? 0 : 1] = p;
				first = !first;
			}
			if (points.Count == 2) {
				path = Pathfinder.GetShortestPath (points[0], points[1]);
				Instance.Line.UpdatePositions (path.ConvertAll (x => x.Position));
			}
		}
	}
}