using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GameInventory;
//using GameActions;
using Pathing;
using GameInput;
using DNA.Tasks;

namespace Units {

	// TODO: Rename to Laborer
	public class Distributor : MobileUnit {

		public override string Name { 
			get { return "Laborer"; }
		}

		public override string Description {
			get { return "Laborers perform work until they reach retirement age."; }
		}

		YearHolder yearHolder = new YearHolder (55, 0);
		//HappinessHolder happinessHolder = new HappinessHolder (100, 100);

		//HappinessIndicator indicator;

		void Awake () {

			Inventory = new Inventory (this);
			Inventory.Add (yearHolder);
			////Inventory.Add (happinessHolder);
			Inventory.Add (new CoffeeHolder (3, 0));
			Inventory.Add (new MilkshakeHolder (5, 0));
			yearHolder.DisplaySettings = new ItemHolderDisplaySettings (true, true);
			//Inventory.Get<HappinessHolder> ().DisplaySettings = new ItemHolderDisplaySettings (true, true);

			PerformableTasks.Add (new CollectItem<MilkshakeHolder> ());
			PerformableTasks.Add (new DeliverItem<MilkshakeHolder> ());
			/*PerformableActions.Add (new CollectItem<MilkshakeHolder> ());
			PerformableActions.Add (new DeliverItem<MilkshakeHolder> ());
			//PerformableActions.Add (new DeliverToPlayer<MilkshakeHolder> ());
			PerformableActions.Add (new CollectItem<CoffeeHolder> ());
			PerformableActions.Add (new DeliverItem<CoffeeHolder> ());
			//PerformableActions.Add (new CollectHappiness ());
			//PerformableActions.Add (new ConsumeItem<HappinessHolder> ());
			PerformableActions.Add (new GenerateItem<YearHolder> ());*/

			Upgrades.Instance.AddListener<CoffeeCapacity> (
				(CoffeeCapacity u) => Inventory["Coffee"].Capacity = u.CurrentValue
			);
		}

		public override void OnPoolCreate () {
			InitInventory ();
			InitPath ();
			InitIndicator ();
			PerformableActions.ActivateAll ();
			RefreshInfoContent ();
		}

		void InitIndicator () {
			//indicator = ObjectCreator.Instance.Create<HappinessIndicator> ().GetScript<HappinessIndicator> ();
			//indicator.Initialize (Transform);
		}

		void InitInventory () {
			Inventory.Empty ();
			/*HappinessHolder happinessHolder = Inventory.Get<HappinessHolder> ();
			happinessHolder.Initialize (100);
			happinessHolder.HolderUpdated += OnHappinessUpdate;*/
			//happinessHolder.HolderUpdated += SetPathSpeed;
			yearHolder.HolderFilled += OnRetirement;
		}

		void InitPath () {
			Path.Active = true;
			Upgrades.Instance.AddListener<DistributorSpeed> (
				(DistributorSpeed u) => Path.Speed = u.CurrentValue / TimerValues.Instance.Year
			);
			//SetPathSpeed ();
		}

		public override void OnPoolDestroy () {
			//ObjectCreator.Instance.Destroy<HappinessIndicator> (indicator.MyTransform);
			//indicator = null;
			PerformableActions.DeactivateAll ();
			//happinessHolder.HolderUpdated -= OnHappinessUpdate;
			yearHolder.HolderFilled -= OnRetirement;
		}

		void OnRetirement () {
			ChangeUnit<Distributor, Elder> ();
		}

		void OnHappinessUpdate () {
			//if (indicator != null) indicator.Fill = happinessHolder.PercentFilled;
		}

		// TODO: Move to MoveOnPath action
		/*void SetPathSpeed () {
			Path.Speed = Mathf.Lerp (
				Path.PathSettings.MinSpeed, 
				Path.PathSettings.MaxSpeed, 
				happinessHolder.PercentFilled) / TimerValues.Instance.Year;
		}*/

		protected override void OnChangeUnit<U> (U u) {
			Path.Active = false;
			Elder elder = u as Elder;
			//elder.AverageHappiness = Inventory.Get<HappinessHolder> ().Average;
			//elder.Init (BoundAcceptor);
			elder.Init (givingTree);
			BoundAcceptor = null;
		}

		/*#if VARIABLE_TIME
		void Update () {
			SetPathSpeed ();
		}
		#endif*/
	}
}