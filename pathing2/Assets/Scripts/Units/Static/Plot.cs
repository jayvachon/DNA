using UnityEngine;
using System.Collections;
using GameInventory;
using GameActions;

namespace Units {
	
	public class Plot : StaticUnit, IActionAcceptor, IActionPerformer {

		public override string Name {
			get { return "Plot"; }
		}
		
		public AcceptableActions AcceptableActions { get; private set; }
		public PerformableActions PerformableActions { get; private set; }

		void Awake () {

			Inventory = new Inventory ();
			Inventory.Add (new MilkshakeHolder (0, 0, CreateBuilding));

			AcceptableActions = new AcceptableActions (this);
			AcceptableActions.Add ("DeliverMilkshake", new AcceptDeliverItem<MilkshakeHolder> ());

		}

		void Start () {
			PerformableActions = new PerformableActions (this);
			PerformableActions.Add ("GenerateHouse", new GenerateUnit<House> (5, Position), "Build House");
			PerformableActions.Add ("GenerateHospital", new GenerateUnit<Hospital> (5, Position), "Build Hospital");
		}

		void CreateBuilding () {
			Debug.Log ("heard");
		}
	}
}