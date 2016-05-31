using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DNA.Units;

namespace DNA.Paths {

	public class GridPoint : PathElement {

		public delegate void OnUpdateConnectionCost ();

		public Vector3 Position { get; set; }

		public Unit Unit {
			get {
				try {
					return (Unit)Object;
				} catch {
					throw new System.Exception ("Object is not type Unit");
				}
			}
		}

		public readonly List<Connection> connections = new List<Connection> ();
		public List<Connection> Connections {
			get { return connections; }
		}

		public bool HasRoad {
			get { return Connections.Find (x => x.Cost == x.Costs["free"]) != null; }
		}

		public bool HasRoadConstruction {
			get { return Connections.Find (x => x.State == DevelopmentState.UnderConstruction) != null; }
		}

		FogOfWar fog = null;
		public FogOfWar Fog {
			get { return fog; }
			set { fog = value; }
		}

		public bool HasFog {
			get { return Fog != null && (Fog.MyState == FogOfWar.State.Covered || Fog.MyState == FogOfWar.State.Faded); }
		}

		public OnUpdateConnectionCost onUpdateConnectionCost;

		public void AddConnection (Connection connection) {
			connection.onUpdateCost += (int c) => { 
				if (onUpdateConnectionCost != null)
					onUpdateConnectionCost (); 
				};
			Connections.Add (connection);
		}
	}
}