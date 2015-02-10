using UnityEngine;
using System.Collections;
using GameInventory;
using GameActions;

public class StaticUnit : MonoBehaviour, IInventoryHolder, IActionPerformer, IActionAcceptor {

	public Inventory Inventory { get; private set; }
	public PerformableActions PerformableActions { get; private set; }
	public AcceptableActions AcceptableActions { get; private set; }

	void Awake () {
			
		Inventory = new Inventory ();
		Inventory.Add (new IceCreamHolder ());
		Inventory.Add (new ElderHolder (5, 2));
		Inventory.Add (new MilkHolder (5, 5));

		PerformableActions = new PerformableActions (this);
		PerformableActions.Add ("GenerateIceCream", new GenerateItem<IceCreamHolder> (3));
		PerformableActions.Add ("ConsumeMilk", new ConsumeItem<MilkHolder> (3));

		AcceptableActions = new AcceptableActions (this);
		AcceptableActions.Add ("CollectIceCream", new AcceptCollectItem<IceCreamHolder> ());
		AcceptableActions.Add ("DeliverElder", new AcceptDeliverItem<ElderHolder> ());
		AcceptableActions.Add ("CollectElder", new AcceptCollectItem<ElderHolder> ());

		// Debugging
		/*Debug.Log ("Static Unit Performable:");
		PerformableActions.Print ();
		Debug.Log ("Static Unit Acceptable:");
		AcceptableActions.Print ();*/
	}
}
