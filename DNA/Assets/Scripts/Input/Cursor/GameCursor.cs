using UnityEngine;
using System.Collections;
using DNA.Units;
using DNA.Tasks;

namespace DNA.InputSystem {

	public static class GameCursor {

		public static Vector3? Target {
			get { return GameCursorMB.Instance.Target; }
			set { GameCursorMB.Instance.Target = value; }
		}

		public static void SetVisual (PerformerTask task, UnitRenderer visual) {
			GameCursorMB.Instance.SetVisual (task, visual);
		}

		public static void RemoveVisual () {
			GameCursorMB.Instance.RemoveVisual ();
		}
	}

	public class GameCursorMB : MonoBehaviour {

		static GameCursorMB instance = null;
		static public GameCursorMB Instance {
			get {
				if (instance == null) {
					instance = Object.FindObjectOfType (typeof (GameCursorMB)) as GameCursorMB;
					if (instance == null) {
						GameObject go = new GameObject ("GameCursorMB");
						DontDestroyOnLoad (go);
						instance = go.AddComponent<GameCursorMB>();
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

		public void SetVisual (PerformerTask task, UnitRenderer newVisual) {
			RemoveVisual ();
			this.visual = newVisual;
			StartCoroutine (CoVisualFollowCursor (task));
		}

		public void RemoveVisual () {
			if (visual != null) ObjectPool.Destroy (visual);
		}

		IEnumerator CoVisualFollowCursor (PerformerTask task) {
		
			while (visual != null) {
				if (Target == null) {
					Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
					RaycastHit hit;
					if (Physics.Raycast (ray, out hit, Mathf.Infinity, 1 << (int)InputLayer.Structure)) {
						visual.transform.position = Vector3.Lerp (ray.origin, hit.point, 0.9f);
					}
				} else {
					visual.transform.position = Vector3.Lerp (visual.transform.position, (Vector3)Target, 0.25f);
				}
				yield return null;
			}
		}
	}
}