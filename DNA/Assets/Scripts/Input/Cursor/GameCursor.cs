using UnityEngine;
using System.Collections;
using DNA.Units;
using DNA.Tasks;

namespace DNA.InputSystem {

	public static class GameCursor {

		public static void SetVisual (PerformerTask task, UnitRenderer visual) {
			GameCursorMB.Instance.SetVisual (task, visual);
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

		public void SetVisual (PerformerTask task, UnitRenderer newVisual) {
			if (visual != null) {
				ObjectPool.Destroy (visual);
			}
			this.visual = newVisual;
			StartCoroutine (CoVisualFollowCursor (task));
		}

		IEnumerator CoVisualFollowCursor (PerformerTask task) {
		
			while (visual != null) {
				Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
				RaycastHit hit;
				if (Physics.Raycast (ray, out hit, Mathf.Infinity, 1 << (int)InputLayer.Structure)) {
					visual.transform.position = Vector3.Lerp (ray.origin, hit.point, 0.9f);
					// Debug.Log (task.Enabled);
				}
				yield return null;
			}
		}
	}
}