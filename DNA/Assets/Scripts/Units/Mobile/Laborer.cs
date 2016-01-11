using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DNA.InputSystem;
using DNA.Tasks;
using DNA.Paths;
using InventorySystem;

namespace DNA.Units {

	public class Laborer : MobileUnit {

		float maxSpeed = 1f;
		float minSpeed = 0.25f;

		void Awake () {

			unitRenderer.SetColors (Palette.Pink);

			Upgrades.Instance.AddListener<CoffeeCapacity> (
				(CoffeeCapacity u) => Inventory["Coffee"].Capacity = u.CurrentValue
			);
			Upgrades.Instance.AddListener<MilkshakeCapacity> (
				(MilkshakeCapacity u) => Inventory["Milkshakes"].Capacity = u.CurrentValue
			);
		}

		protected override void OnInitInventory (Inventory i) {
			i.Add (new YearGroup (0, 1000));//.onFill += OnRetirement; <-- Uncomment to have laborers turn into elders on retirement
			i.Add (new CoffeeGroup (0, 3));
			i.Add (new MilkshakeGroup (0, 5));
			i.Add (new LaborGroup (0, 1000));
			i.Add (new HappinessGroup (100, 100)).onUpdate += OnUpdateHappiness;
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

		void OnUpdateHappiness () {
			if (positioner == null) return;
			int happiness = Inventory["Happiness"].Count;
			if (happiness % 10 == 0) {
				float p = Inventory["Happiness"].PercentFilled;
				unitRenderer.SetColors (Color.Lerp (Palette.Blue, Palette.Pink, p));
				positioner.Speed = Mathf.Lerp (minSpeed, maxSpeed, p);
			}
		}

		protected override void OnChangeUnit<Unit> (Unit u) {
			Elder e = u as Elder;
			if (e != null) {
				GridPoint startPoint = CurrentPoint;
				if (startPoint == null) {
					ConnectionContainer c = ConnectionsManager.FindNearest (Position);
					startPoint = c.Connection.Points[0];
				}
				e.SetStartPoint (startPoint, false);
			}
		}
	}
}