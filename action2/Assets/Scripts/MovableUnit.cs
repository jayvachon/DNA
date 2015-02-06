using UnityEngine;
using System.Collections;
using GameActions;
using GameInventory;

public class MovableUnit : MonoBehaviour, IActionPerformer {

	// Debugging
	public StaticUnit boundUnit;

	public PerformableActions PerformableActions { get; private set; }

	void Awake () {

		PerformableActions = new PerformableActions (this);
		PerformableActions.Add ("CollectElder", new CollectItem<ElderItem> (3));
		PerformableActions.Add ("DeliverElder", new DeliverItem<ElderItem> (3));
		PerformableActions.Activate ("DeliverElder");
		PerformableActions.Activate ("CollectElder");

		// Debugging
		/*Debug.Log("Movable Unit:");
		PerformableActions.Print ();*/
	}

	void Start () {
		IActionAcceptor acceptor = boundUnit as IActionAcceptor;
		ActionMatcher matcher = new ActionMatcher ();
		matcher.MatchActions (PerformableActions, acceptor.AcceptableActions);
		matcher.Print ();
	}

	public void OnEndAction () {}
}
