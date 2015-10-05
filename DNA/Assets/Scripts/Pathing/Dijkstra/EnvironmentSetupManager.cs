using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DNA.Paths;
using DNA.Units;

namespace DNA {

	public class EnvironmentSetupManager : MonoBehaviour {

		public PointsManager points;
		public ConnectionsManager connections;

		void Awake () {
			
			points.Init ();
			connections.Init ();

			// Create Giving Tree
			points.SetUnitAtIndex<GivingTreeUnit> (0);
			
			// Create initial roads
			List<Connection> topConnections = points.GetConnectionsAtIndex (0);
			for (int i = 0; i < topConnections.Count; i ++) {
				topConnections[i].SetCost ("free");
			}
		}
	}
}