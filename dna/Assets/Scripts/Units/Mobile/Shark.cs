using UnityEngine;
using System.Collections;

namespace DNA.Units {

	public class Shark : Unit {

		GivingTreeUnit givingTree;

		public static Shark Create (Vector3 position, GivingTreeUnit givingTree) {
			Shark shark = ObjectPool.Instantiate<Shark> (position);
			shark.Init (givingTree);
			return shark;
		}

		void Init (GivingTreeUnit givingTree) {
			
			this.givingTree = givingTree;
			
			Vector3 startPosition = Position;
			Vector3 targetPosition = givingTree.Position;
			Vector3 dir = (startPosition - targetPosition).normalized;
			targetPosition += dir * 3;
			
			float distance = Vector3.Distance (startPosition, targetPosition);
			float speed = 5f;

			Co2.StartCoroutine (distance / speed, (float p) => {
				Position = Vector3.Lerp (startPosition, targetPosition, p);
				MyTransform.LookAt (targetPosition);
			});
		}
	}
}