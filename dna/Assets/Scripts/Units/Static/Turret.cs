using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace DNA.Units {

	public class Turret : StaticUnit, IDamager, IWorkplace {
		
		public bool Accessible { get; set; }
		public float Efficiency { get; set; }

		float range = 7.5f;
		RangeRing ring;
		Lazer lazer;
		Shark target;

		bool UpdateTarget () {
			
			List<Shark> sharks = UnitManager.GetUnitsOfType<Shark> ();

			// If a target has already been found, keep shooting it
			if (target != null && sharks.Contains (target))
				return false;

			// Otherwise, find the nearest target
			Shark nearest = null;
			float nearestDistance = Mathf.Infinity;
			
			foreach (Shark shark in sharks) {
				float distance = Vector3.Distance (shark.Position, Position);
				if (distance <= range && distance < nearestDistance) {
					nearest = shark;
					nearestDistance = distance;
				}
			}

			// Return true if the target was updated
			bool updated = target != nearest;
			target = nearest;
			return updated;
		}

		protected override void OnEnable () {

			base.OnEnable ();

			lazer = Lazer.Create (MyTransform, new Vector3 (0, 1.5f, 0));
			ring = RangeRing.Create (MyTransform);
			ring.Set (range, 40);
			ring.Hide ();

			Co2.InvokeWhileTrue (0.5f, () => { return gameObject.activeSelf; }, () => {
				if (UpdateTarget ()) {
					if (target == null) {
						lazer.StopFire ();
					} else {
						lazer.StartFire (target.MyTransform);
						target.StartTakeDamage ();
					}
				}
			});
		}

		public override void OnSelect () {
			base.OnSelect ();
			ring.Show ();
		}

		public override void OnUnselect () {
			base.OnUnselect ();
			ring.Hide ();
		}
	}
}