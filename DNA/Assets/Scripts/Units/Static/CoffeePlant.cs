﻿using UnityEngine;
using System.Collections;
using DNA.InputSystem;
using DNA.Tasks;
using InventorySystem;

namespace DNA.Units {

	public class CoffeePlant : StaticUnit, ITaskPerformer {
		
		PerformableTasks performableTasks;
		public PerformableTasks PerformableTasks {
			get {
				if (performableTasks == null) {
					performableTasks = new PerformableTasks (this);
					PerformableTasks.Add (new GenerateItem<CoffeeGroup> ());
					/*PerformableTasks.Add (new GenerateItem<YearGroup> ()).onComplete += (PerformerTask t) => { 
						if (Element != null)
							Element.State = DevelopmentState.Abandoned; 
						PerformableTasks[typeof (GenerateItem<CoffeeGroup>)].Stop ();
					};*/
				}
				return performableTasks;
			}
		}

		protected override void OnInitInventory (Inventory i) {
			// i.Add (new CoffeeGroup (0, 10));
			i.Add (new CoffeeGroup ());
		}

		protected override void OnInitAcceptableTasks (AcceptableTasks a) {
			a.Add (new AcceptCollectItem<CoffeeGroup> ());				
		}

		protected override void OnEnable () {
			base.OnEnable ();
			// Inventory["Years"].Clear ();
			// Inventory["Coffee"].Set (2);
			// PerformableTasks[typeof (GenerateItem<YearGroup>)].Start ();
			// PerformableTasks[typeof (GenerateItem<CoffeeGroup>)].Start ();
		}

		protected override void OnDisable () {
			base.OnDisable ();
			// PerformableTasks[typeof (GenerateItem<CoffeeGroup>)].Stop ();
			// PerformableTasks[typeof (GenerateItem<YearGroup>)].Stop ();
		}

		protected override void OnSetFertility (int tier) {
			int amount = (int)(2000 * Fertility.Multipliers[tier]);
			Inventory["Coffee"].Capacity = amount;
			Debug.Log (amount);
			// Inventory["Coffee"].Set (2);
			Inventory["Coffee"].Fill ();
		}
	}
}