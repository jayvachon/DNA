using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DNA.EventSystem;
using Pathing;

namespace DNA.Paths {

	[RequireComponent (typeof (LineDrawer))]
	public class RoadConstructor : MBRefs {

		// TODO: track user's actions instead of making this a singleton (which only calls the Build method)

		static RoadConstructor instance = null;
		static public RoadConstructor Instance {
			get {
				if (instance == null) {
					instance = Object.FindObjectOfType (typeof (RoadConstructor)) as RoadConstructor;
					if (instance == null) {
						GameObject go = new GameObject ("RoadConstructor");
						instance = go.AddComponent<RoadConstructor>();
					}
				}
				return instance;
			}
		}

		public int NewSegmentCount {
			get {
				
				if (connections == null || connections.Count == 0)
					return 0;
				
				int count = 0;
				foreach (Connection c in connections) {
					if (c.Cost > 0) count ++;
				}
				return count;
			}
		}

		LineDrawer drawer = null;
		LineDrawer Drawer {
			get {
				if (drawer == null) {
					drawer = GetComponent<LineDrawer> ();
				}
				return drawer;
			}
		}

		readonly List<GridPoint> points = new List<GridPoint> ();
		readonly List<GridPoint> path = new List<GridPoint> ();
		List<Connection> connections;

		public void Build () {
			foreach (Connection c in connections)
				c.SetCost ("free");
			Clear ();
		}

		void OnEnable () { PlayerActionState.onChange += OnChangeState; }
		void OnDisable () { PlayerActionState.onChange -= OnChangeState; }

		void OnClickPointEvent (ClickPointEvent e) {

			// If both points have roads, find shortest route
			// If second point does not have a road, find cheapest route

			GridPoint newPoint = e.Point;

			if (points.Count == 0) {
				if (!newPoint.HasRoad)
					return;
				points.Add (newPoint);
				ObjectPool.Instantiate ("ConstructionStartIndicator", newPoint.Position);
			} else if (points.Count == 1) {
				if (points.Contains (newPoint)) {
					// toggle
					return;
				} else {
					ObjectPool.Instantiate ("ConstructionEndIndicator", newPoint.Position);
					points.Add (newPoint);
					if (newPoint.HasRoad)
						GenerateShortestPath ();
					else
						GenerateCheapestPath ();
				}
			}
		}

		void GenerateShortestPath () {
			path.Clear ();
			for (int i = 0; i < points.Count-1; i ++)
				path.AddRange (Pathfinder.GetShortestPath (points[i], points[i+1]));
			connections = Pathfinder.PointsToConnections (path);
			Drawer.UpdatePositions (path.ConvertAll (x => x.Position));
		}

		void GenerateCheapestPath () {
			path.Clear ();
			for (int i = 0; i < points.Count-1; i ++)
				path.AddRange (Pathfinder.GetCheapestPath (points[i], points[i+1]));
			connections = Pathfinder.PointsToConnections (path);
			Drawer.UpdatePositions (path.ConvertAll (x => x.Position));
		}

		void Clear () {
			path.Clear ();
			points.Clear ();
			Drawer.Clear ();
			ObjectPool.Destroy ("ConstructionStartIndicator");
			ObjectPool.Destroy ("ConstructionEndIndicator");
		}

		void OnChangeState (ActionState state) {
			if (state == ActionState.RoadConstruction) {
				Events.instance.AddListener<ClickPointEvent> (OnClickPointEvent);
			} else {
				Events.instance.RemoveListener<ClickPointEvent> (OnClickPointEvent);
				Clear ();
			}
		}
	}
}