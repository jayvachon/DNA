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
			Inventory.Add (new MilkshakeHolder (100, 0));

			AcceptableActions = new AcceptableActions (this);
			AcceptableActions.Add ("DeliverMilkshake", new AcceptDeliverItem<MilkshakeHolder> ());

			PerformableActions = new PerformableActions (this);
			PerformableActions.Add ("GenerateUnit", new GenerateUnit<Distributor> (5));
		}

		/*void Update () {
			if (Input.GetKeyDown (KeyCode.Space)) {
				UnitCreator.instance.Create<Distributor> (Vector3.zero);
			}
		}*/
	}
}