using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Delaunay;
using Delaunay.Geo;
using DNA.Paths.Dijkstra;

namespace DNA.Paths {

	public class RoadsManager : MBRefs {

		new void Awake () {
			CreateRoads ();
		}

		void CreateRoads () {
			foreach (Connection c in TreeGrid.Connections) {
				Road road = ObjectCreator.Instance.Create<Road> ().GetScript<Road> ();
				//road.SetPoints (c.Positions[0], c.Positions[1]);
				road.Connection = c;
				road.Parent = MyTransform;
			}
		}
	}
}