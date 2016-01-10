using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DNA.InputSystem;
using DNA.Tasks;
using InventorySystem;

namespace DNA.Units {

	public class Laborer : MobileUnit {

		void Awake () {

			unitRenderer.SetColors (new Color (1f, 0.5f, 1f));

			Upgrades.Instance.AddListener<CoffeeCapacity> (
				(CoffeeCapacity u) => Inventory["Coffee"].Capacity = u.CurrentValue
			);
		}

		protected override void OnInitInventory (Inventory i) {
			i.Add (new YearGroup (0, 1000)).onFill += OnRetirement;
			i.Add (new CoffeeGroup (0, 3));
			i.Add (new MilkshakeGroup (0, 5));
			i.Add (new LaborGroup (0, 1000));
			i.Add (new HappinessGroup (100, 100));
		}

		protected override void OnInitPerformableTasks (PerformableTasks p) {
			p.Add (new CollectItem<MilkshakeGroup> ());
			p.Add (new DeliverItem<MilkshakeGroup> ());
			p.Add (new CollectItem<CoffeeGroup> ());
			p.Add (new DeliverItem<CoffeeGroup> ());
			p.Add (new GenerateItem<YearGroup> ());
			p.Add (new CollectItem<LaborGroup> ()).onEnd += (PerformerTask t) => { Inventory["Labor"].Clear (); };
			p.Add (new CollectItem<HappinessGroup> ());
			p.Add (new ConsumeItem<HappinessGroup> ());
		}

		protected override void OnEnable () {
			Inventory.Clear ();
			Inventory["Happiness"].Fill ();
			RefreshInfoContent ();
			PerformableTasks[typeof (GenerateItem<YearGroup>)].Start ();
			base.OnEnable ();
		}

		protected override void OnDisable () {
			base.OnDisable ();
			PerformableTasks[typeof (GenerateItem<YearGroup>)].Stop ();
		}

		void OnRetirement () {
			ChangeUnit<Laborer, Elder> ();
		}

		protected override void OnChangeUnit<U> (U u) {
			//Path.Active = false;
			//Elder elder = u as Elder;
			//elder.Init (givingTree);
		}
	}
}