using UnityEngine;
using System.Collections;
using InventorySystem;

namespace DNA.Units {

	public class Shark : Unit, IDamageable, IDamager {

		Lazer lazer;
		ProgressBar pbar;
		float damageTimer = 0f;
		const float damageTime = 5f;

		public static Shark Create (Vector3 position, GivingTreeUnit givingTree, Loan loan) {
			Shark shark = ObjectPool.Instantiate<Shark> (position);
			shark.Init (givingTree, loan);
			return shark;
		}

		void Init (GivingTreeUnit givingTree, Loan loan) {
			
			// Create progress bar
			if (pbar == null)
				pbar = UI.Instance.CreateProgressBar (MyTransform, new Vector3 (0, -0.5f, 0));
			pbar.SetProgress (Inventory["Health"].PercentFilled);

			// Create laser
			if (lazer == null)
				lazer = Lazer.Create (this);

			// Initialize inventory
			// loan.Transfer (Inventory[loan.ID], loan.Payment);
			// Inventory[loan.Group.ID].Set (loan.Payment);

			// Set trajectory
			Vector3 startPosition = Position;
			Vector3 targetPosition = givingTree.Position;
			Vector3 dir = (startPosition - targetPosition).normalized;
			targetPosition += dir * 3;
			startPosition.y += 2;
			targetPosition.y += 2;
			
			float distance = Vector3.Distance (startPosition, targetPosition);
			float speed = 5f;

			// Move
			Co2.StartCoroutine (distance / speed, (float p) => {
				Position = Vector3.Lerp (startPosition, targetPosition, p);
				MyTransform.LookAt (targetPosition);
			}, () => {
				lazer.StartFire (givingTree, new Vector3 (0, 2, 0));
			});
		}

		protected override void OnInitInventory (Inventory i) {
			i.Add (new CoffeeGroup ());
			i.Add (new MilkshakeGroup ());
			i.Add (new HealthGroup (100, 100)).onEmpty += () => {
				DestroyThis<Shark> ();
			};
		}

		protected override void OnEnable () {
			base.OnEnable ();
			Inventory["Health"].Fill ();
		}

		protected override void OnDisable () {
			base.OnDisable ();
			lazer.StopFire ();
			Inventory.Clear ();
		}

		public void TakeDamage (IDamager damager) {
			damageTimer += Time.deltaTime;
			if (damageTimer >= damageTime / 100f) {
				Inventory["Health"].Remove ();
				pbar.SetProgress (Inventory["Health"].PercentFilled);
				damageTimer = 0f;
			}
		}
	}
}