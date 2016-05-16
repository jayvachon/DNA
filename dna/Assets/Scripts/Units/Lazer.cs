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

		// IDamager damager;
		// IDamageable target;

		/*void Init (IDamager damager) {
			this.damager = damager;
		}*/

		// public void StartFire (IDamageable target, Vector3 offset=new Vector3 ()) {
		public void StartFire (Transform target, Vector3 offset=new Vector3 ()) {

			// this.target = target;
			LineRenderer.enabled = true;
			// Transform targetTransform = target.transform;
			// target.StartTakeDamage (damager);

			// Co2.RunWhileTrue (() => { return gameObject.activeSelf && targetTransform.gameObject.activeSelf; }, () => {
			Co2.RunWhileTrue (() => { return gameObject.activeSelf && target.gameObject.activeSelf; }, () => {
				LineRenderer.SetPositions (new Vector3[] {
					Position,
					// targetTransform.position + offset
					target.position + offset
				});
			}, StopFire);
		}

		public void StopFire () {
			LineRenderer.enabled = false;
			// target.StopTakeDamage ();
		}

		// public static Lazer Create (IDamager damager, Vector3 offset=new Vector3()) {
		public static Lazer Create (Transform damager, Vector3 offset=new Vector3()) {

			// Transform parentTransform = ((MonoBehaviour)damager).transform;

			Lazer lazer = ObjectPool.Instantiate<Lazer> ();
			lazer.transform.SetParent (damager);
			lazer.transform.Reset ();
			lazer.transform.localPosition = offset;
			// lazer.Init (damager);

			return lazer;
		}
	}
}