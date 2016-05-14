using UnityEngine;
using System.Collections;
using InventorySystem;

namespace DNA.Units {

	public class Shark : Unit {

		Lazer lazer;

		public static Shark Create (Vector3 position, GivingTreeUnit givingTree) {
			Shark shark = ObjectPool.Instantiate<Shark> (position);
			shark.Init (givingTree);
			return shark;
		}

		void Init (GivingTreeUnit givingTree) {
			
			Vector3 startPosition = Position;
			Vector3 targetPosition = givingTree.Position;
			Vector3 dir = (startPosition - targetPosition).normalized;
			targetPosition += dir * 3;
			startPosition.y += 2;
			targetPosition.y += 2;
			
			float distance = Vector3.Distance (startPosition, targetPosition);
			float speed = 5f;

			if (lazer == null)
				lazer = Lazer.Create (MyTransform);

			Co2.StartCoroutine (distance / speed, (float p) => {
				Position = Vector3.Lerp (startPosition, targetPosition, p);
				MyTransform.LookAt (targetPosition);
			}, () => {
				lazer.StartFire (givingTree.transform, new Vector3 (0, 2, 0));
			});
		}

		protected override void OnInitInventory (Inventory i) {
			i.Add (new HealthGroup (100, 100));
		}
	}
}