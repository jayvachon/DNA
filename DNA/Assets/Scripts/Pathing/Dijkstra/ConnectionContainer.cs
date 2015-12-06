using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using DNA.EventSystem;
using DNA.InputSystem;
using DNA.Units;

namespace DNA.Paths {

	[RequireComponent (typeof (BoxCollider))]
	public class ConnectionContainer : PathElementContainer {

		Connection connection;
		public Connection Connection {
			get { return connection; }
			set {
				connection = value;
				SetPosition (connection.Positions[0], connection.Positions[1]);
				Element = connection;
				Collider.size = new Vector3 (1f, 0.1f, connection.Length);
			}
		}

		protected override Vector3 Anchor {
			get { return MyTransform.InverseTransformPoint (Connection.Center); }
		}

		new BoxCollider collider = null;
		BoxCollider Collider {
			get {
				if (collider == null) {
					collider = GetComponent<BoxCollider> ();
				}
				return collider;
			}
		}

		bool ColliderEnabled {
			get { return Collider.enabled; }
			set { Collider.enabled = value; }
		}

		void OnEnable () {
			// ColliderEnabled = false;
		}

		void SetPosition (Vector3 a, Vector3 b) {
			// Position = a;
			Position = Vector3.Lerp (a, b, 0.5f);
			MyTransform.LookAt (b);
			OnSetPoints ();
		}

		protected virtual void OnSetPoints () {}

		#region IPointerDownHandler implementation
		public override void OnPointerDown (PointerEventData e) {
			Events.instance.Raise (new ClickConnectionEvent (this));
			if (Connection.Object is Unit)
				SelectionHandler.ClickSelectable ((ISelectable)Connection.Object, e);
		}
		#endregion

		protected override void OnSetObject (IPathElementObject obj) {
			Road r = obj as Road;
			if (r != null) {
				connection.SetCost ("free");
				r.Init (Connection.Length);
			}
		}
	}
}