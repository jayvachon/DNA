using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DNA.Paths;
using DNA.Units;

namespace DNA {

	public class EnvironmentSetupManager : MonoBehaviour {

		public PointsManager points;
		public ConnectionsManager connections;
		public FogOfWarManager fogOfWar;

		#if UNITY_EDITOR
		public bool createFlower = false;
		public bool createUniversity = false;
		public bool createTurret = false;
		public bool createDerrick = false;
		public bool createRoads = false;
		#endif

		void Awake () {
			
			points.OnLoadPoints += OnLoadPoints;
			connections.OnLoadConnections += OnLoadConnections;
			fogOfWar.OnLoadFog += OnLoadFog;
			points.Init ();
			connections.Init ();
		}

		void OnLoadPoints () {
			CreateDrillablePlots ();
			CreateCoffeePlants ();
		}

		void OnLoadFog () {

			points.SetUnitAtIndex<GivingTreeUnit> (0);

			#if UNITY_EDITOR
			if (createFlower)
				points.SetUnitAtIndex<Flower> (2);
			if (createUniversity)
				points.SetUnitAtIndex<University> (3);
			if (createTurret)
				points.SetUnitAtIndex<Turret> (4);
			if (createDerrick)
				points.SetUnitAtIndex<MilkshakePool> (15);

			if (createRoads) {
				ConnectionContainer c = ConnectionsManager.GetContainer (points.GetConnectionsAtIndex (2)[3]);
				c.BeginConstruction<Road> ();
				c.EndConstruction ();
			}

			#endif
		}

		void CreateDrillablePlots () {

			float pointCount = (float)(points.Points.Count);
			float index = 0;
			const float perlinScale = 20f;
			List<PointContainer> containers = points.Points;

			foreach (PointContainer pc in containers) {
				float distanceToCenter = index / pointCount;
				float val = Mathf.PerlinNoise (
					((pc.Point.Position.x / 100f) + 1f) / 2f * perlinScale,
					((pc.Point.Position.z / 100f) + 1f) / 2f * perlinScale
				);
				pc.SetFertility (distanceToCenter, val);
				index ++;
			}

			int drillableCount = (int)(points.Points.Count * 0.08f);
			Dictionary<float, int> vals = new Dictionary<float, int> ();
			for (int i = 0; i < containers.Count; i ++)
				vals.Add (QuasiRandom (), i);

			foreach (var val in vals.OrderBy (k => Mathf.Abs (k.Key))) {
				if (drillableCount > 0) {
					containers[val.Value].SetObject<DrillablePlot> ();
					drillableCount --;
				} else {
					containers[val.Value].SetObject<Plot> ();
				}
			}
		}

		void CreateCoffeePlants () {

			int first = Random.Range (6, 20);
			if (first == 15) first += 1;
			points.SetUnitAtIndex<CoffeePlant> (first);

			int pointsCount = points.Points.Count;
			int coffeeCount = (int)(pointsCount * 0.08f);

			Dictionary<float, int> vals = new Dictionary<float, int> ();

			for (int i = 20; i < pointsCount; i ++)
				vals.Add (QuasiRandom (), i);

			foreach (var val in vals.OrderBy (k => Mathf.Abs (k.Key))) {
				points.SetUnitAtIndex<CoffeePlant> (val.Value);
				coffeeCount --;
				if (coffeeCount == 0) break;
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

		float QuasiRandom () {

			float U, u, v, S;

		    do
		    {
		        u = 2.0f * UnityEngine.Random.value - 1.0f;
		        v = 2.0f * UnityEngine.Random.value - 1.0f;
		        S = u * u + v * v;
		    }
		    while (S >= 1.0f);

		    float fac = Mathf.Sqrt(-2.0f * Mathf.Log(S) / S);
		    return u * fac;
		}
	}
}