using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using DNA.Paths;
using DNA.InputSystem;
using DNA.EventSystem;

namespace DNA {

	public class FogOfWar : MBRefs, IPointerDownHandler {

		GridPoint point;
		public GridPoint Point {
			get { return point; }
			set {
				
				point = value;
				point.Fog = this;
				point.OnSetState += OnSetState;
				
				neighbors[0] = new List<GridPoint> ();
				foreach (Connection c in point.Connections) {
					GridPoint other = System.Array.Find (c.Points, x => x != point);
					other.OnSetState += OnNeighborSetState1;
					neighbors[0].Add (other);
				}

				for (int i = 1; i < neighbors.Length; i ++) {
					neighbors[i] = new List<GridPoint> ();
					foreach (GridPoint p in neighbors[i-1]) {
						foreach (Connection c in p.Connections) {
							GridPoint a = c.Points[0];
							GridPoint b = c.Points[1];
							if (!neighbors[i-1].Contains (a) && !neighbors[i].Contains (a) && a != point) {
								neighbors[i].Add (a);
								if (i == 1)
									a.OnSetState += OnNeighborSetState2;
								if (i == 2)
									a.OnSetState += OnNeighborSetState3;
							}
							if (!neighbors[i-1].Contains (b) && !neighbors[i].Contains (b) && b != point) {
								neighbors[i].Add (b);	
								if (i == 1)
									b.OnSetState += OnNeighborSetState2;
								if (i == 2)
									b.OnSetState += OnNeighborSetState3;
							}
						}
					}
				}
			}
		}

		List<GridPoint>[] neighbors = new List<GridPoint>[3];

		public enum FogState { Solid, Faded, Empty }
		FogState state = FogState.Empty;
		public FogState State {
			get { return state; }
			set {
				state = value;
				if (onUpdateState != null)
					onUpdateState (state);
			}
		}

		public delegate void OnUpdateState (FogState state);
		public OnUpdateState onUpdateState;

		void OnEnable () {
			GetComponent<Renderer> ().SetColor (Palette.YellowGreen);
			State = FogState.Solid;
		}
		
		void OnSetState (DevelopmentState state) { RemoveIfDeveloped (state); }
		void OnNeighborSetState1 (DevelopmentState state) { RemoveIfDeveloped (state); }
		void OnNeighborSetState2 (DevelopmentState state) { FadeIfDeveloped (state); }
		void OnNeighborSetState3 (DevelopmentState state) { FadeIfDeveloped (state); }

		void FadeIfDeveloped (DevelopmentState devState) {
			if (devState == DevelopmentState.Developed) {
				GetComponent<Renderer> ().SetColor (Palette.ApplyAlpha (Palette.Green, 0.3f));
				State = FogState.Faded;
			}
		}

		void RemoveIfDeveloped (DevelopmentState devState) {
			if (devState == DevelopmentState.Developed) {
				Point.Fog = null;
				ObjectPool.Destroy (this);
				RemoveListeners ();
				State = FogState.Empty;
			}
		}

		void RemoveListeners () {
			for (int i = 0; i < neighbors.Length; i ++) {
				foreach (GridPoint p in neighbors[i]) {
					if (i == 0)
						p.OnSetState -= OnNeighborSetState1;
					if (i == 1)
						p.OnSetState -= OnNeighborSetState2;
					if (i == 2)
						p.OnSetState -= OnNeighborSetState3;
				}
			}
		}

		#region IPointerDownHandler implementation
		public void OnPointerDown (PointerEventData e) {
			Events.instance.Raise (new PointerDownEvent (this));
		}
		#endregion
	}
}