using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GameActions;
using GameInventory;

public class MovableUnit : MonoBehaviour, IActionPerformer {

	// Debugging
	public StaticUnit boundUnit;

	public PerformableActions PerformableActions { get; private set; }

	void Awake () {

		PerformableActions = new PerformableActions (this);
		PerformableActions.Add ("CollectElder", new CollectItem<ElderHolder> (3));
		PerformableActions.Add ("DeliverElder", new DeliverItem<ElderHolder> (3));
		PerformableActions.Enable ("DeliverElder");
		PerformableActions.Enable ("CollectElder");

		// Debugging
		/*Debug.Log("Movable Unit:");
		PerformableActions.Print ();*/
	}

	void Start () {
		ActionHandler.instance.Bind (this, boundUnit as IActionAcceptor);
	}

	public void OnEndAction () {}
}
