using UnityEngine;
using System.Collections;
using GameActions;
using GameInventory;

public class StaticUnit : MonoBehaviour, IActionPerformer, IActionAcceptor {

	public PerformableActions PerformableActions { get; private set; }
	public AcceptableActions AcceptableActions { get; private set; }

	void Awake () {
		
		PerformableActions = new PerformableActions (this);
		PerformableActions.Add ("GenerateIceCream", new GenerateItem<IceCreamItem> (3));

		AcceptableActions = new AcceptableActions ();
		AcceptableActions.Add ("CollectIceCream");
		AcceptableActions.Add ("DeliverElder");
		AcceptableActions.Add ("CollectElder");

		// Debugging
		/*Debug.Log ("Static Unit Performable:");
		PerformableActions.Print ();
		Debug.Log ("Static Unit Acceptable:");
		AcceptableActions.Print ();*/
	}

	public void OnEndAction () {}
}
