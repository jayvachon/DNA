using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DNA.Units;

namespace DNA.Paths {

	public class GridPoint : PathElement {

		public Vector3 Position { get; set; }

		public readonly List<Connection> connections = new List<Connection> ();
		public List<Connection> Connections {
			get { return connections; }
		}

		public bool HasRoad {
			get { return Connections.Find (x => x.Cost == x.Costs["free"]) != null; }
		}
	}
}