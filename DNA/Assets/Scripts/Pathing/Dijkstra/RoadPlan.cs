using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

// deprecate

namespace DNA.Paths {

	public class RoadPlan {

		public GridPoint Terminus { get; private set; }

		List<Connection> connections;

		public RoadPlan (Connection connection) {
			this.connections = new List<Connection> { connection };
			connection.OnSetState += OnSetConnectionState;
			UpdateTerminus ();
		}

		public RoadPlan (List<Connection> connections) {
			this.connections = connections;
			foreach (Connection connection in connections) {
				connection.OnSetState += OnSetConnectionState;
			}
			UpdateTerminus ();
		}

		void OnSetConnectionState (DevelopmentState state) {
			if (state == DevelopmentState.Developed) {
				connections.Remove (connections.FirstOrDefault (x => x.State == DevelopmentState.Developed));
				if (connections.Count > 0)
					UpdateTerminus ();
			}
		}

		void UpdateTerminus () {
			Connection c = connections.FirstOrDefault (x => x.Points[0].HasRoad || x.Points[1].HasRoad);
			Terminus = c.Points[0].HasRoad ? c.Points[0] : c.Points[1];
		}
	}
}