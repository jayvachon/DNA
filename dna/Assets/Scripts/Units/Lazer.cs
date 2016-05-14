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

		IDamager damager;

		void Init (IDamager damager) {
			this.damager = damager;
		}

		public void StartFire (IDamageable target, Vector3 offset=new Vector3 ()) {

			LineRenderer.enabled = true;
			Transform targetTransform = ((MonoBehaviour)target).transform;

			Co2.RunWhileTrue (() => { return gameObject.activeSelf && targetTransform.gameObject.activeSelf; }, () => {
				LineRenderer.SetPositions (new Vector3[] {
					Position,
					targetTransform.position + offset
				});
				target.TakeDamage (damager);
			}, StopFire);
		}

		public void StopFire () {
			LineRenderer.enabled = false;
		}

		public static Lazer Create (IDamager damager, Vector3 offset=new Vector3()) {

			Transform parentTransform = ((MonoBehaviour)damager).transform;

			Lazer lazer = ObjectPool.Instantiate<Lazer> ();
			lazer.transform.SetParent (parentTransform);
			lazer.transform.Reset ();
			lazer.transform.localPosition = offset;
			lazer.Init (damager);

			return lazer;
		}
	}
}