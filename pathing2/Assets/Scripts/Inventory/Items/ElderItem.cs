using UnityEngine;
using System.Collections;
using GameActions;

namespace GameInventory {

	public class ElderItem : Item, IActionPerformer {

		float health = 1f;
		public float Health {
			get { return HealthManager.Health; }
			//set { health = value; }
		}

		HealthManager healthManager = new HealthManager ();
		public HealthManager HealthManager {
			get { return healthManager; }
		}

		public PerformableActions PerformableActions { get; private set; }

		public ElderItem () {
			PerformableActions = new PerformableActions (this);
			PerformableActions.Add ("SubtractHealth", new SubtractHealth ());
		}

		public override void Print () {
			Debug.Log ("Elder Health: " + health);
		}
	}
}