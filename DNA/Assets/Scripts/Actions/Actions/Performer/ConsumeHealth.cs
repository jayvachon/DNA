using UnityEngine;
using System.Collections;
using GameInventory;

namespace GameActions {

	public class ConsumeHealth : ConsumeItem<HealthHolder> {

		HealthManager2 healthManager;

		public ConsumeHealth (HealthManager2 healthManager) : base (healthManager.DegradeRate) {
			this.healthManager = healthManager;
			this.healthManager.onChangeDegradeRate += OnChangeDegradeRate;
			OnChangeDegradeRate ();
		}

		void OnChangeDegradeRate () {
			Debug.Log (Duration);
			Duration = healthManager.DegradeRate;
		}
	}
}