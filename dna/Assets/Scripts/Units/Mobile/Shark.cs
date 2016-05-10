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
			float distance = Vector3.Distance (startPosition, targetPosition);
			Co2.StartCoroutine (1f / distance, (float p) => {
				Position = Vector3.Lerp (startPosition, targetPosition, p);
			});
		}
	}
}