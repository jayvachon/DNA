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

		public void Build () {
			List<Road> roads = Pathfinder.PointsToConnections (path).ConvertAll (x => x.Road);
			foreach (Road r in roads) {
				r.Build ();
			}
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
			Drawer.UpdatePositions (path.ConvertAll (x => x.Position));
		}

		void Clear () {
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