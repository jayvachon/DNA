using UnityEngine;
using System.Collections;
using GameActions;

namespace GameInventory {

	public class ElderItem : Item, IActionPerformer {

		float health = 1f;
		public float Health {
			get { return health; }
			set { health = value; }
		}

		//AgeManager ageManager = new AgeManager ();
		HealthManager healthManager = new HealthManager ();

		public PerformableActions PerformableActions { get; private set; }

		public ElderItem () {
			
			health = Random.Range (0f, 1f);
			PerformableActions = new PerformableActions (this);
			PerformableActions.Add ("SubtractHealth", new SubtractHealth (8));

			Debug.Log (health);
		}

		public void SubtractHealth (float amount) {
			if (health > amount) {
				health -= amount;
				Debug.Log (health);
			} else {
				Holder.Remove (this);
			}
		}

		public override void Print () {
			Debug.Log ("Elder Health: " + health);
		}
	}
}