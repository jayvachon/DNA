using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GameEvents;

namespace GameInput {

	public class MouseController : MonoBehaviour {

		public static Vector3 MousePosition {
			get { return ScreenPositionHandler.ScreenToWorld (Input.mousePosition); }
		}

		public static Vector3 MousePositionWorld {
			get {
				Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
				RaycastHit hit;
				if (Physics.Raycast (ray, out hit, Mathf.Infinity)) {
					return hit.point;
				}
				return Vector3.zero;
			}
		}

		static int layer = -1;
		public static int Layer {
			set {
				layer = value;
			}
			get {
				if (layer == -1) {
					layer = LayerController.DefaultLayer;
				}
				return layer;
			}
		}

		public static int IgnoreLayers {
			get { return LayerController.IgnoreLayers; }
		}

		ClickManager clickManager;
		DragManager dragManager;
		ReleaseManager releaseManager;

		void Awake () {
			int[] layers = LayerController.Layers;
			clickManager = new ClickManager (layers);
			dragManager = new DragManager (layers);
			releaseManager = new ReleaseManager (layers);
		}

		void LateUpdate () {
			if (Input.GetMouseButton (0)) {
				clickManager.HandleMouseDown (0);
				dragManager.HandleMouseDown (0);
				releaseManager.HandleMouseDown (0);
			}
			if (!Input.GetMouseButton (0)) {
				clickManager.HandleMouseUp (0);
				dragManager.HandleMouseUp (0);
				releaseManager.HandleMouseUp (0);
			}
			if (Input.GetMouseButton (1)) {
				clickManager.HandleMouseDown (1);
				dragManager.HandleMouseDown (1);
				releaseManager.HandleMouseDown (1);
			}
			if (!Input.GetMouseButton (1)) {
				clickManager.HandleMouseUp (1);
				dragManager.HandleMouseUp (1);
				releaseManager.HandleMouseUp (1);
			}
		}

		#if UNITY_EDITOR
		void Update () {
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			RaycastHit hit;
			if (Physics.Raycast (ray, out hit, Mathf.Infinity)) {
				Debug.DrawRay (ray.origin, ray.direction * 1000);
			} 
		}
		#endif
	}
}