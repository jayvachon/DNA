using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Delaunay;
using Delaunay.Geo;

namespace DNA.Paths {

	public class RoadsManager : MonoBehaviour {

		List<GridPoint> path;
		int index = 0;

		void Awake () {
			//CreateRoads ();
			//PathfindingTest ();
		}

		void CreateRoads () {
			foreach (GridPoint[] connection in TreeGrid.Connections) {
				Road road = ObjectCreator.Instance.Create<Road> ().GetScript<Road> ();
				road.SetPoints (connection[0].Position, connection[1].Position);
			}
		}

		void Update () {
			if (Input.GetKeyDown (KeyCode.Space)) {
				index ++;
				path = Pathfinder.GetShortestPath (TreeGrid.Points[0], TreeGrid.Points[index]);;
				Debug.Log (path.Count);
			}
		}

		/*void PathfindingTest () {
			path = Pathfinder.GetShortestPath (TreeGrid.Points[0], TreeGrid.Points[12]);
			foreach (GridPoint gp in path) {
				Debug.Log (gp.Position);
			}
		}*/

		void OnDrawGizmos () {
			if (path != null) {
				Gizmos.color = Color.yellow;
				for (int i = 0; i < path.Count-1; i ++) {
					Gizmos.DrawLine (path[i].Position, path[i+1].Position);
				}
			}
		}
	}
}