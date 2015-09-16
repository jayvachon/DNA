using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Delaunay;
using Delaunay.Geo;

public class RoadsManager : MonoBehaviour {

	void Awake () {
		foreach (LineSegment edge in TreeGrid.Connections) {
			List<Vector3> v3edge = TreeGrid.EdgeToV3 (edge);
			Road road = ObjectCreator.Instance.Create<Road> ().GetScript<Road> ();
			road.SetPoints (v3edge[0], v3edge[1]);
		}
	}

	
}
