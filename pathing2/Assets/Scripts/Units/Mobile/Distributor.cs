using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GameInventory;
using GameActions;
using Pathing;
using GameInput;

namespace Units {

	// TODO: Rename to Laborer
	public class Distributor : MobileUnit {

		public override string Name { 
			get { return "Laborer"; }
		}

		Jacuzzi boundJacuzzi = null;
		YearHolder yearHolder = new YearHolder (55, 0);
		HappinessHolder happinessHolder = new HappinessHolder (100, 100);

		HappinessIndicator indicator;

		void Awake () {

			Inventory = new Inventory (this);
			Inventory.Add (yearHolder);
			Inventory.Add (happinessHolder);
			Inventory.Add (new CoffeeHolder (3, 0));
			Inventory.Add (new MilkshakeHolder (5, 0));
			yearHolder.DisplaySettings = new ItemHolderDisplaySettings (true, true);
			Inventory.Get<HappinessHolder> ().DisplaySettings = new ItemHolderDisplaySettings (true, true);

			PerformableActions = new PerformableActions (this);
			PerformableActions.Add (new CollectItem<MilkshakeHolder> ());
			PerformableActions.Add (new DeliverItem<MilkshakeHolder> ());
			PerformableActions.Add (new CollectItem<CoffeeHolder> ());
			PerformableActions.Add (new DeliverItem<CoffeeHolder> ());
			PerformableActions.Add (new DeliverUnpairedItem<DistributorHolder> ());
			PerformableActions.Add (new CollectHappiness ());
			PerformableActions.Add (new ConsumeItem<HappinessHolder> ());
			PerformableActions.Add (new GenerateItem<YearHolder> ());
		}

		public override void OnPoolCreate () {
			InitInventory ();
			InitPath ();
			InitIndicator ();
			PerformableActions.ActivateAll ();
			UnitInfoContent.Refresh ();
		}

		void InitIndicator () {
			indicator = ObjectCreator.Instance.Create<HappinessIndicator> ().GetScript<HappinessIndicator> ();
			indicator.Parent = Transform;
			indicator.MyTransform.SetLocalPosition (new Vector3 (0f, 1f, 0f));
		}

		void InitInventory () {
			Inventory.Empty ();
			HappinessHolder happinessHolder = Inventory.Get<HappinessHolder> ();
			happinessHolder.Initialize (100);
			happinessHolder.HolderUpdated += OnHappinessUpdate;
			yearHolder.HolderUpdated += OnAge;
			yearHolder.HolderFilled += OnRetirement;
		}

		void InitPath () {
			Path.Active = true;
			SetPathSpeed ();
		}

		public override void OnPoolDestroy () {
			ObjectCreator.Instance.Destroy<HappinessIndicator> (indicator.MyTransform);
			PerformableActions.DeactivateAll ();
			yearHolder.HolderUpdated -= OnAge;
			yearHolder.HolderFilled -= OnRetirement;
		}

		void OnAge () {
			SetPathSpeed ();
		}

		void OnRetirement () {
			ChangeUnit<Distributor, Elder> ();
		}

		void OnHappinessUpdate () {
			indicator.Fill = happinessHolder.PercentFilled;
		}

		public override bool OnBindActionable (IActionAcceptor acceptor) {
			UpdateEfficiency ();
			return base.OnBindActionable (acceptor);
		}

		protected override void OnBind () {
			UnbindJacuzzi ();
			Jacuzzi jacuzzi = BoundAcceptor as Jacuzzi;
			if (jacuzzi != null) {
				boundJacuzzi = jacuzzi;
				PerformableActions.SetActive ("DeliverDistributor", false);
			}
		}

		protected override void OnUnbind () {
			UnbindJacuzzi ();
		}

		void UnbindJacuzzi () {
			if (boundJacuzzi != null) {
				boundJacuzzi.Inventory.RemoveItem<DistributorHolder> ();
				PerformableActions.SetActive ("DeliverDistributor", true);
				boundJacuzzi = null;
			}
		}

		void UpdateEfficiency () {
			float efficiency = Mathf.Max (0.1f, Inventory.Get<HappinessHolder> ().PercentFilled);
			PerformableActions.Get ("CollectMilkshake").Efficiency = efficiency;
			PerformableActions.Get ("DeliverMilkshake").Efficiency = efficiency;
			PerformableActions.Get ("CollectCoffee").Efficiency = efficiency;
			PerformableActions.Get ("DeliverCoffee").Efficiency = efficiency;
		}

		void SetPathSpeed () {
			float progress = yearHolder.PercentFilled;
			float p = Mathf.Clamp01 (Mathf.Abs (progress - 1));
			Path.Speed = Path.PathSettings.MaxSpeed * Mathf.Sqrt(-(p - 2) * p) / TimerValues.Instance.Year;
		}

		protected override void OnChangeUnit<U> (U u) {
			Path.Active = false;
			Elder elder = u as Elder;
			elder.AverageHappiness = Inventory.Get<HappinessHolder> ().Average;
		}

		#if VARIABLE_TIME
		void Update () {
			SetPathSpeed ();
		}
		#endif
	}
}