using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DNA.Paths;

namespace DNA {

	public class FlowerMesh : MonoBehaviour {

		Mesh mesh;
		Vector3[] vertices;

		void Awake () {
			InitMesh ();
		}

		void InitMesh () {
			mesh = new Mesh ();
			InitPoints ();
		}

		void InitPoints () {
			List<Connection> connections = TreeGrid.Connections;
			foreach (Connection c in connections) {
				GizmosDrawer.Instance.Add (new GizmoLine (c.Positions[0], c.Positions[1]));
			}
			List<GridPoint> points = TreeGrid.Points;
			vertices = points.ConvertAll (x => x.Position).ToArray ();
			mesh.vertices = vertices;
		}
	}
}