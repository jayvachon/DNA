using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Delaunay;
using Delaunay.Geo;
using DNA.Paths.Dijkstra;

namespace DNA.Paths {

	public class RoadsManager : MonoBehaviour {

		List<GridPoint> path;
		int index = 0;

		void Awake () {
			//CreateRoads ();
		}

		void CreateRoads () {
			foreach (Connection c in TreeGrid.Connections) {
				Road road = ObjectCreator.Instance.Create<Road> ().GetScript<Road> ();
				road.SetPoints (c.Positions[0], c.Positions[1]);
			}
		}

		void Update () {
			if (Input.GetKeyDown (KeyCode.Space)) {
				index ++;
				path = Pathfinder.GetShortestPath (TreeGrid.Points[0], TreeGrid.Points[index]);;
			}
			if (Input.GetKeyDown (KeyCode.A)) {
				// 264
				// 244
				TreeGrid.Connections[244].SetFree ();
				//TreeGrid.Connections[244].DisablePath ();
			}
		}

		void OnDrawGizmos () {
			Gizmos.color = Color.black;
			if (path != null) {
				for (int i = 0; i < path.Count-1; i ++) {
					Gizmos.DrawLine (path[i].Position, path[i+1].Position);
				}
			}
			Gizmos.DrawLine (TreeGrid.Connections[244].Positions[0], TreeGrid.Connections[244].Positions[1]);
		}
	}
}