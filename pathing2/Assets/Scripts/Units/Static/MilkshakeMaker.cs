using UnityEngine;
using System.Collections;
using GameInventory;
using GameActions;

namespace Units {

	public class MilkshakeMaker : StaticUnit, IActionPerformer {

		public override string Name {
			get { return "Milkshake Maker"; }
		}
		
		public PerformableActions PerformableActions { get; private set; }

		void Awake () {
			
			Inventory = new Inventory (this);
			Inventory.Add (new MilkHolder (5, 0));
			Inventory.Add (new IceCreamHolder (5, 0));
			Inventory.Add (new MilkshakeHolder (10, 0));

			AcceptableActions = new AcceptableActions (this);
			AcceptableActions.Add (new AcceptDeliverItem<MilkHolder> ());
			AcceptableActions.Add (new AcceptDeliverItem<IceCreamHolder> ());
			AcceptableActions.Add (new AcceptCollectItem<MilkshakeHolder> ());

			PerformableActions = new PerformableActions (this);
			PerformableActions.Add (new CombineItems<MilkHolder, IceCreamHolder, MilkshakeHolder> (5));
		}
	}
}