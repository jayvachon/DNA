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

		int FadeLevel {
			get { return Upgrades.Instance.GetUpgrade<Eyesight> ().CurrentValue; }
		}

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
		bool hasConstruction = false;

		public State MyState {
			get { return state; }
		}

		List<List<Elements>> rings = new List<List<Elements>> ();

		Color myColor = Color.black;
		Color MyColor {
			get {
				if (myColor == Color.black) {
					float range = 0.1f;
					myColor = new Color (RandomInRange (0.698f,range), 0.153f, RandomInRange (0.905f,range), 0.3f);
				}
				return myColor;
			}
		}

		void OnEnable () {
			hasConstruction = false;
			Renderer.enabled = true;
			Collider.enabled = true;
			Renderer.SetColor (MyColor);
			GetComponent<Renderer> ().SetAlpha (1f);
			state = State.Covered;
			Upgrades.Instance.AddListener<Eyesight> (UpgradeFadeLevel);
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
		}

		void Remove () {
			Hide ();
			GetRing (FadeLevel);
			int fadeRingCount = 2;
			for (int i = 0; i < Mathf.Max (0, FadeLevel-fadeRingCount); i ++) {
				foreach (Elements e in rings[i]) {
					e.Point.Fog.Hide ();
				}
			}
			for (int i = FadeLevel-1; i >= fadeRingCount; i --) {
				foreach (Elements e in rings[i]) {
					e.Point.Fog.Fade ();
				}
			}
			state = State.Removed;
			hasConstruction = true;
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

		void Hide () {
			Renderer.enabled = false;
			Collider.enabled = false;
			state = State.Hidden;
		}

		void Fade () {
			GetComponent<Renderer> ().SetColor (Palette.ApplyAlpha (MyColor, 0.5f));
			state = State.Faded;
		}

		float RandomInRange (float val, float range) {
			return val + ((Random.value > 0.5f) ? (range * Random.value) : (-range * Random.value));
		}

		void UpgradeFadeLevel (Eyesight u) {
			if (hasConstruction)
				Remove ();
		}
		
		#region IPointerDownHandler implementation
		public void OnPointerDown (PointerEventData e) {
			Events.instance.Raise (new PointerDownEvent (this, e));
		}
		#endregion
	}
}