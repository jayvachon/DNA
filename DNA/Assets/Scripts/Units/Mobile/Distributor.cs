using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DNA.InventorySystem;
using DNA.InputSystem;
using DNA.Tasks;

namespace DNA.Units {

	// TODO: Rename to Laborer
	public class Distributor : MobileUnit {

		public override string Name { 
			get { return "Laborer"; }
		}

		public override string Description {
			get { return "Laborers perform work until they reach retirement age."; }
		}

		YearHolder yearHolder = new YearHolder (350, 0);

		void Awake () {

			unitRenderer.SetColors (new Color (1f, 0.5f, 1f));

			Inventory = new Inventory (this);
			Inventory.Add (yearHolder);
			Inventory.Add (new CoffeeHolder (3, 0));
			Inventory.Add (new MilkshakeHolder (5, 0));
			Inventory.Add (new LaborHolder (1, 0));
			yearHolder.DisplaySettings = new ItemHolderDisplaySettings (true, true);

			PerformableTasks.Add (new CollectItem<MilkshakeHolder> ());
			PerformableTasks.Add (new DeliverItem<MilkshakeHolder> ());
			PerformableTasks.Add (new CollectItem<CoffeeHolder> ());
			PerformableTasks.Add (new DeliverItem<CoffeeHolder> ());
			PerformableTasks.Add (new GenerateItem<YearHolder> ());
			PerformableTasks.Add (new CollectItem<LaborHolder> ()).onEnd += (PerformerTask t) => { Inventory["Labor"].Clear (); };

			Upgrades.Instance.AddListener<CoffeeCapacity> (
				(CoffeeCapacity u) => Inventory["Coffee"].Capacity = u.CurrentValue
			);
		}

		//public override void OnPoolCreate () {
		protected override void OnEnable () {
			InitInventory ();
			//InitPath ();
			RefreshInfoContent ();
			PerformableTasks[typeof (GenerateItem<YearHolder>)].Start ();
			base.OnEnable ();
		}

		void InitInventory () {
			Inventory.Empty ();
			yearHolder.HolderFilled += OnRetirement;
		}

		/*void InitPath () {
			Path.Active = true;
			Upgrades.Instance.AddListener<DistributorSpeed> (
				(DistributorSpeed u) => Path.Speed = u.CurrentValue / TimerValues.Instance.Year
			);
		}*/

		//public override void OnPoolDestroy () {
		protected override void OnDisable () {
			base.OnDisable ();
			yearHolder.HolderFilled -= OnRetirement;
			PerformableTasks[typeof (GenerateItem<YearHolder>)].Stop ();
		}

		void OnRetirement () {
			ChangeUnit<Distributor, Elder> ();
		}

		protected override void OnChangeUnit<U> (U u) {
			//Path.Active = false;
			//Elder elder = u as Elder;
			//elder.Init (givingTree);
		}
	}
}