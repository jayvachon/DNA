using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using DNA.Units;
using DNA.Tasks;
using DNA.EventSystem;

namespace DNA.InputSystem {

	public class GameCursor : MonoBehaviour {

		static GameCursor instance = null;
		static public GameCursor Instance {
			get {
				if (instance == null) {
					instance = Object.FindObjectOfType (typeof (GameCursor)) as GameCursor;
					if (instance == null) {
						GameObject go = new GameObject ("GameCursor");
						DontDestroyOnLoad (go);
						instance = go.AddComponent<GameCursor>();
					}
				}
				return instance;
			}
		}

		UnitRenderer visual;

		Vector3? target = null;
		public Vector3? Target {
			get { return target; }
			set { target = value; }
		}

		Quaternion rotation = Quaternion.identity;
		public Quaternion Rotation {
			get { return rotation; }
			set { rotation = value; }
		}

		public delegate void OnClick (bool overTarget);
		public OnClick onClick;

		public MainCamera cam;

		void OnEnable () { Events.instance.AddListener<PointerDownEvent> (OnPointerDownEvent); }
		void OnDisable () { Events.instance.RemoveListener<PointerDownEvent> (OnPointerDownEvent); }

		public void SetVisual (PerformerTask task, UnitRenderer newVisual) {
			RemoveVisual ();
			this.visual = newVisual;
			StartCoroutine (CoVisualFollowCursor (task));
		}

		public void RemoveVisual () {
			if (visual != null) {
				ObjectPool.Destroy (visual);
				visual = null;
			}
		}

		void OnPointerDownEvent (PointerDownEvent e) {
			if (onClick != null)
				onClick (Target != null);
			if (e.data.button == PointerEventData.InputButton.Right) {
				/*Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
				RaycastHit hit;
				if (Physics.Raycast (ray, out hit, Mathf.Infinity, 1 << (int)InputLayer.Structure)) {
					StartCoroutine (CoMoveDrag (hit.point));
				}*/
				StartCoroutine (CoMoveDrag (Input.mousePosition));
			}
		}

		// TODO: move to maincamera
		IEnumerator CoMoveDrag (Vector3 start) {
			
			float startRotation = cam.center.localEulerAngles.y;
			float startPosition = cam.LocalPosition.z;

			while (Input.GetMouseButton (1)) {
				Vector3 delta = cam.Camera.ScreenToViewportPoint (Input.mousePosition - start);
				cam.center.SetLocalEulerAnglesY (startRotation + delta.x * 90);
				// TODO: smoothing
				cam.transform.SetLocalPositionZ (startPosition - delta.y * 30);
				yield return null;
			}
		}

		IEnumerator CoVisualFollowCursor (PerformerTask task) {

			while (visual != null) {
				Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
				RaycastHit hit;
				if (Target == null) {
					if (Physics.Raycast (ray, out hit, Mathf.Infinity, 1 << (int)InputLayer.Structure)) {
						visual.transform.position = Vector3.Lerp (ray.origin, hit.point, 0.9f);
						visual.transform.rotation = Quaternion.Lerp (visual.transform.rotation, Quaternion.identity, 0.5f);
					}
				} else {
					visual.transform.position = Vector3.Lerp (visual.transform.position, (Vector3)Target, 0.25f);
					visual.transform.rotation = Quaternion.Lerp (visual.transform.rotation, Rotation, 0.5f);
				}
				yield return null;
			}
		}
	}
}