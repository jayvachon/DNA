using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using DNA.Paths;
using DNA.InputSystem;
using DNA.EventSystem;
using DNA.Units;

namespace DNA {

	public class FogOfWar : MBRefs, IPointerDownHandler {

		static int fadeLevel = 2;

		GridPoint point;
		public GridPoint Point {
			get { return point; }
			set {
				point = value;
				point.Fog = this;
			}
		}

		Renderer _renderer = null;
		Renderer Renderer {
			get {
				if (_renderer == null) {
					_renderer = GetComponent<Renderer> ();
				}
				return _renderer;
			}
		}

		Collider _collider = null;
		Collider Collider {
			get {
				if (_collider == null) {
					_collider = GetComponent<Collider> ();
				}
				return _collider;
			}
		}

		struct Elements {

			public readonly GridPoint Point;
			public readonly Connection Connection;

			public Elements (GridPoint point, Connection connection) {
				Point = point;
				Connection = connection;
			}
		}

		public enum State { Removed, Hidden, Faded, Covered }
		State state = State.Covered;

		public State MyState {
			get { return state; }
		}

		List<List<Elements>> rings = new List<List<Elements>> ();

		void OnEnable () {
			Renderer.enabled = true;
			Collider.enabled = true;
			Renderer.SetColor (Palette.YellowGreen);
			state = State.Covered;
		}
		
		public void Init () {

			rings.Add (GetOuterRing (new List<Elements> { new Elements (Point, null) }));

			foreach (Elements e in rings[0]) {
				Connection c = e.Connection;
				if (c.Object != null && c.Object is Road) {
					Remove ();
				} else {
					c.OnSetObject += OnSetObject;
				}
			}

			if (Point.Object != null && Point.Object is Unit && Point.Unit.Settings.RemovesFogOfWar) {
				Remove ();
			} else {
				Point.OnSetObject += OnSetObject;
			}

			// this crashes the project
			// Upgrades.Instance.AddListener<Eyesight> (UpgradeFadeLevel);
		}

		void Remove () {
			Hide ();
			GetRing (fadeLevel);
			for (int i = 0; i < fadeLevel-1; i ++) {
				foreach (Elements e in rings[i]) {
					e.Point.Fog.Hide ();
				}
			}
			foreach (Elements e in rings[fadeLevel-1]) {
				e.Point.Fog.Fade ();
			}
			state = State.Removed;
		}

		List<Elements> GetRing (int index) {

			while (rings.Count-1 < index) {
				List<Elements> newRing = GetOuterRing (rings[rings.Count-1]).FindAll (x => !PointContainedInRings (x.Point));
				rings.Add (newRing);
			}

			return rings[index];
		}

		List<Elements> GetOuterRing (List<Elements> ring) {
			List<Elements> outerRing = new List<Elements> ();
			foreach (Elements e in ring) {
				outerRing.AddRange (GetNeighbors (e.Point));
			}
			return outerRing;
		}

		List<Elements> GetNeighbors (GridPoint p) {
			List<Elements> neighbors = new List<Elements> ();
			List<Connection> connections = p.Connections;
			foreach (Connection c in connections) {
				neighbors.Add (new Elements (c.GetOtherPoint (p), c));
			}
			return neighbors;
		}

		bool PointContainedInRings (GridPoint p) {
			foreach (List<Elements> ring in rings) {
				foreach (Elements elements in ring) {
					if (elements.Point == p)
						return true;
				}
			}
			return false;
		}

		void OnSetObject (IPathElementObject obj) {
			Unit u = obj as Unit;
			if (obj is Road || (u != null && u.Settings.RemovesFogOfWar)) {
				Remove ();
			}
		}

		public void Hide () {
			Renderer.enabled = false;
			Collider.enabled = false;
			state = State.Hidden;
		}

		public void Fade () {
			GetComponent<Renderer> ().SetColor (Palette.ApplyAlpha (Palette.Green, 0.3f));
			state = State.Faded;
		}

		void UpgradeFadeLevel (Eyesight u) {
			fadeLevel += 1;
			List<FogOfWar> fow = ObjectPool.GetActiveInstances<FogOfWar> ();
			foreach (FogOfWar f in fow) {
				if (f.MyState == FogOfWar.State.Removed) {
					f.Remove ();
				}
			}
			// TODO: update current fade levels on upgrade (right now, need to wait for road construction to finish)			
		}
		
		#region IPointerDownHandler implementation
		public void OnPointerDown (PointerEventData e) {
			Events.instance.Raise (new PointerDownEvent (this, e));
		}
		#endregion
	}
}