using UnityEngine;
using System.Collections;

namespace DNA.Units {

	[RequireComponent (typeof (LineRenderer))]
	public class Lazer : MBRefs {

		LineRenderer lineRenderer = null;
		LineRenderer LineRenderer {
			get {
				if (lineRenderer == null) {
					lineRenderer = GetComponent<LineRenderer> ();
				}
				return lineRenderer;
			}
		}

		Transform target;

		public void StartFire (Transform target, Vector3 offset=new Vector3 ()) {

			this.target = target;

			LineRenderer.enabled = true;

			Co2.RunWhileTrue (() => { return gameObject.activeSelf; }, () => {
				LineRenderer.SetPositions (new Vector3[] {
					Position,
					target.position + offset
				});
			});
		}

		public void StopFire () {
			LineRenderer.enabled = false;
		}

		public static Lazer Create (Transform parent, Vector3 offset=new Vector3()) {
			Lazer lazer = ObjectPool.Instantiate<Lazer> ();
			lazer.transform.SetParent (parent);
			lazer.transform.Reset ();
			lazer.transform.localPosition = offset;
			return lazer;
		}
	}
}