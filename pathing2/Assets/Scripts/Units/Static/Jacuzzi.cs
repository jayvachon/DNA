using UnityEngine;
using System.Collections;
using GameInventory;
using GameActions;

namespace Units {

	public class Jacuzzi : StaticUnit, IActionAcceptor, IActionPerformer {

		public override string Name {
			get { return "Jacuzzi"; }
		}
		
		public AcceptableActions AcceptableActions { get; private set; }
		public PerformableActions PerformableActions { get; private set; }

		void Awake () {
			
			Inventory = new Inventory (this);
			Inventory.Add (new HappinessHolder (500, 500));

			AcceptableActions = new AcceptableActions (this);
			AcceptableActions.Add ("CollectHappiness", new AcceptCollectItem<HappinessHolder> ());

			PerformableActions = new PerformableActions (this);
			PerformableActions.Add ("GenerateHappiness", new GenerateItem<HappinessHolder> (1));
		}
	}
}