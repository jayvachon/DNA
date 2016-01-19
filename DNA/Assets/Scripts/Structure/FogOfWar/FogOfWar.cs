using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using DNA.Paths;
using DNA.InputSystem;
using DNA.EventSystem;

namespace DNA {

	public class FogOfWar : MBRefs, IPointerDownHandler {

		static int fadeLevel;
		public static int FadeLevel {
			get { return fadeLevel; }
			private set { fadeLevel = value; }
		}

		GridPoint point;
		public GridPoint Point {
			get { return point; }
			set {

				point = value;
				point.Fog = this;

				List<Connection> connections = point.Connections;

				foreach (Connection c in connections) {
					c.onUpdateCost += (int cost) => {
						OnUpdateConnection (cost);
						RemoveNeighbors ();
					};
				}
			}
		}

		void OnEnable () {
			GetComponent<Renderer> ().SetColor (Palette.YellowGreen);
		}
		
		public void Init () {
			if (Point.Connections.Find (x => x.Cost == 0) != null) {
				OnUpdateConnection (0);
				RemoveNeighbors ();
			}
		}

		public void Remove () {
			Point.Fog = null;
			ObjectPool.Destroy (this);
			foreach (Connection c in Point.Connections) {
				GridPoint other = c.GetOtherPoint (Point);
				FogOfWar f = other.Fog;
				if (f != null) {
					f.Fade ();
				}
				if (fadeLevel > 0) {
					foreach (Connection c2 in other.Connections) {
						GridPoint other2 = c2.GetOtherPoint (Point);
						FogOfWar f2 = other2.Fog;
						if (f2 != null) {
							f2.Fade ();
						}
					}
				}
			}
		}

		public void Fade () {
			GetComponent<Renderer> ().SetColor (Palette.ApplyAlpha (Palette.Green, 0.3f));
		}

		void OnUpdateConnection (int cost) {
			if (cost == 0) Remove ();
		}

		void RemoveNeighbors () {
			List<GridPoint> n = Point.Connections.ConvertAll (x => x.GetOtherPoint (Point));
			foreach (GridPoint gp in n) {
				if (gp.Fog != null)
					gp.Fog.Remove ();
			}
		}

		public static void UpgradeFadeLevel () {
			FadeLevel += 1;
			// TODO: update current fade levels on upgrade (right now, need to wait for road construction to finish)			
		}
		
		#region IPointerDownHandler implementation
		public void OnPointerDown (PointerEventData e) {
			Events.instance.Raise (new PointerDownEvent (this));
		}
		#endregion
	}
}