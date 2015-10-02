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

			// If the point already exists in the list, remove it (toggle)
			// If the point already exists on the path, ignore it

			if (points.Contains (e.Point)) {
				points.Remove (e.Point);
			} else {
				if (path.Contains (e.Point))
					return;
				points.Add (e.Point);
			}
			GeneratePath ();
		}

		void GeneratePath () {
			path.Clear ();
			for (int i = 0; i < points.Count-1; i ++)
				path.AddRange (Pathfinder.GetShortestPath (points[i], points[i+1]));
			connections = Pathfinder.PointsToConnections (path);
			Drawer.UpdatePositions (path.ConvertAll (x => x.Position));
		}

		void Clear () {
			path.Clear ();
			points.Clear ();
			Drawer.Clear ();
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