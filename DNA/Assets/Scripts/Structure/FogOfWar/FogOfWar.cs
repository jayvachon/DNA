using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DNA.Paths;

namespace DNA {

	public class FogOfWar : MBRefs {

		GridPoint point;
		public GridPoint Point {
			get { return point; }
			set {
				point = value;
				point.OnSetState += OnSetState;
				foreach (Connection c in point.Connections) {
					GridPoint other = System.Array.Find (c.Points, x => x != point);
					other.OnSetState += OnNeighborSetState;
					neighbors.Add (other);
				}
				foreach (GridPoint n in neighbors) {
					foreach (Connection c in n.Connections) {
						GridPoint a = c.Points[0];
						GridPoint b = c.Points[1];
						if (!neighbors.Contains (a) && a != point) {
							neighbors2.Add (a);
							a.OnSetState += OnNeighbor2SetState;
						}
						if (!neighbors.Contains (b) && b != point) {
							neighbors2.Add (b);	
							b.OnSetState += OnNeighbor2SetState;
						}
					}
				}
			}
		}

		List<GridPoint> neighbors = new List<GridPoint> ();
		List<GridPoint> neighbors2 = new List<GridPoint> ();

		protected override void Awake () {
			GetComponent<Renderer> ().SetColor (new Color (0, 0, 0, 1));
		}

		void OnSetState (DevelopmentState state) {
			if (state == DevelopmentState.Developed) {
				ObjectPool.Destroy (this);
				RemoveListeners ();
			}
		}

		void OnNeighborSetState (DevelopmentState state) {
			if (state == DevelopmentState.Developed) {
				ObjectPool.Destroy (this);
				RemoveListeners ();
			}
		}

		void OnNeighbor2SetState (DevelopmentState state) {
			if (state == DevelopmentState.Developed)
				GetComponent<Renderer> ().SetColor (new Color (0, 0, 0, 0.5f));
		}

		void RemoveListeners () {
			foreach (GridPoint n in neighbors) {
				n.OnSetState -= OnNeighborSetState;
			}
			foreach (GridPoint n in neighbors2) {
				n.OnSetState -= OnNeighbor2SetState;
			}
		}
	}
}