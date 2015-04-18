using UnityEngine;
using System.Collections;
using GameActions;
using Units;

namespace GameInventory {

	public class ElderItem : Item, IActionPerformer {

		class Settings {
			public bool CanBecomeSickWhileMoving = false;
		}
		Settings settings = new Settings ();

		float health = 1f;
		public float Health {
			get { return HealthManager.Health; }
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

		public override void OnAdd () {
			if (settings.CanBecomeSickWhileMoving) return;
			if (Inventory == null) {
				// This is null if the ItemHolder adds items in its constructor
				// there probably exists a better way of handling this, but it's fine for now i think?
				return;
			}
			if (Inventory.holder is MobileUnit) {
				HealthManager.CanBecomeSick = false;
			} else if (Inventory.holder is StaticUnit) {
				HealthManager.CanBecomeSick = true;
			}
		}

		public override void Print () {
			Debug.Log ("Elder Health: " + health);
		}
	}
}