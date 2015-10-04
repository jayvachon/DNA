using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

namespace DNA.Paths {

	[RequireComponent (typeof (BoxCollider))]
	public class ConnectionContainer : MBRefs, IPointerDownHandler {

		Connection connection;
		public Connection Connection {
			get { return connection; }
			set {
				connection = value;
				connection.onUpdateCost += OnUpdateCost;
				SetPosition (connection.Positions[0], connection.Positions[1]);
			}
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
			ColliderEnabled = false;
		}

		void CreateRoad () {
			Road r = ObjectPool.Instantiate<Road> ();
			Connection.Road = r;
			r.MyTransform.SetParent (MyTransform);
			r.MyTransform.localPosition = Vector3.zero;
			r.MyTransform.rotation = MyTransform.rotation;
			r.MyTransform.localScale = MyTransform.localScale;
			r.Init (Connection.Length);
		}

		void SetPosition (Vector3 a, Vector3 b) {
			Position = a;
			MyTransform.LookAt (b);
			OnSetPoints ();
		}

		protected virtual void OnSetPoints () {}

		void OnUpdateCost (int cost) {
			if (cost == 0 && Connection.Road == null)
				CreateRoad ();
		}

		#region IPointerDownHandler implementation
		public void OnPointerDown (PointerEventData e) {}
		#endregion
	}
}