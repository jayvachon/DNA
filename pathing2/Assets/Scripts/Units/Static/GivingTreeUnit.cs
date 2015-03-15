using UnityEngine;
using System.Collections;
using GameInventory;
using GameActions;

namespace Units {

	public class GivingTreeUnit : StaticUnit, IActionAcceptor, IActionPerformer {

		public override string Name {
			get { return "Giving Tree"; }
		}

		public AcceptableActions AcceptableActions { get; private set; }
		public PerformableActions PerformableActions { get; private set; }

		void Awake () {

			Inventory = new Inventory ();
			Inventory.Add (new MilkshakeHolder (100, 10));

			AcceptableActions = new AcceptableActions (this);
			AcceptableActions.Add ("DeliverMilkshake", new AcceptDeliverItem<MilkshakeHolder> ());

			PerformableActions = new PerformableActions (this);
			Vector3 createPosition = StaticTransform.Position;
			createPosition.x -= 2;
			
			PerformableActions.Add ("GenerateDistributor", new GenerateUnit<Distributor, MilkshakeHolder> (5, createPosition), "Birth Distributor");
		}
	}
}