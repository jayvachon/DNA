using UnityEngine;
using System.Collections;
using GameInventory;
using GameActions;

namespace Units {

	public class MilkshakeMaker : StaticUnit, IActionAcceptor, IActionPerformer {

		public override string Name {
			get { return "Milkshake Maker"; }
		}
		
		public AcceptableActions AcceptableActions { get; private set; }
		public PerformableActions PerformableActions { get; private set; }

		void Awake () {
			
			Inventory = new Inventory ();
			Inventory.Add (new MilkHolder (5, 0));
			Inventory.Add (new IceCreamHolder (5, 0));
			Inventory.Add (new MilkshakeHolder (10, 0));

			AcceptableActions = new AcceptableActions (this);
			AcceptableActions.Add ("DeliverMilk", new AcceptDeliverItem<MilkHolder> ());
			AcceptableActions.Add ("DeliverIceCream", new AcceptDeliverItem<IceCreamHolder> ());
			AcceptableActions.Add ("CollectMilkshake", new AcceptCollectItem<MilkshakeHolder> ());

			PerformableActions = new PerformableActions (this);
			PerformableActions.Add ("CombineMilkAndIceCream", new CombineItems<MilkHolder, IceCreamHolder, MilkshakeHolder> (5));
		}
	}
}