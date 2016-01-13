using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DNA.Paths;
using DNA.Units;

namespace DNA {

	public class EnvironmentSetupManager : MonoBehaviour {

		public PointsManager points;
		public ConnectionsManager connections;
		public FogOfWarManager fogOfWar;

		void Awake () {
			
			points.OnLoadPoints += OnLoadPoints;
			connections.OnLoadConnections += OnLoadConnections;
			fogOfWar.OnLoadFog += OnLoadFog;
			points.Init ();
			connections.Init ();
		}

		void OnLoadPoints () {
			CreateCoffeePlants ();
		}

		void OnLoadFog () {
			points.SetUnitAtIndex<GivingTreeUnit> (0);
			points.SetUnitAtIndex<MilkshakePool> (2);
			// points.SetUnitAtIndex<CoffeePlant> (2);
			// points.SetUnitAtIndex<Flower> (4);
		}

		void CreateCoffeePlants () {

			int first = Random.Range (6, 20);
			points.SetUnitAtIndex<CoffeePlant> (first);

			int pointsCount = points.Points.Count;
			for (int i = 20; i < pointsCount; i ++) {
				if (Random.value < 0.1f) {
					points.SetUnitAtIndex<CoffeePlant> (i);
				}
			}
		}

		void OnLoadConnections () {
			
			// Create initial roads
			List<Connection> topConnections = points.GetConnectionsAtIndex (0);
			for (int i = 0; i < topConnections.Count; i ++) {
				ConnectionContainer c = ConnectionsManager.GetContainer (topConnections[i]);
				c.BeginConstruction<Road> ();
				c.EndConstruction ();
			}

			/*List<Connection> topConnections2 = points.GetConnectionsAtIndex (12);
			for (int i = 0; i < topConnections2.Count; i ++) {
				ConnectionContainer d = ConnectionsManager.GetContainer (topConnections2[i]);
				d.BeginConstruction<Road> ();
				d.EndConstruction ();
			}*/

			fogOfWar.Init ();
		}
	}
}