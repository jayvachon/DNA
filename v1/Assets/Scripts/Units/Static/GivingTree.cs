using UnityEngine;
using System.Collections;
using GameActions;

public class GivingTree : StaticUnit, IActionPerformer {

	public PerformableActions PerformableActions { get; private set; }

	protected override void Awake () {
		base.Awake ();

		PerformableActions = new PerformableActions (this);
		PerformableActions.Add ("GenerateDistributor", new GenerateUnit<Distributor> (3));
	}

	/**
	 *	Debugging
	 */

	void Update () {
		// TODO: This works, but the instantiated movable unit isn't a child of the mover
		// the way to handle this is to uncouple the mesh renderer from the movable unit and
		// have it be a child of the movable unit (same as the mover)
		// movable unit should only handle actions, inventory, path, etc- nothing visual or
		// transform-related
		if (Input.GetKeyDown (KeyCode.Q)) {
			GenerateUnit<Distributor> gen = PerformableActions.Get ("GenerateDistributor") as GenerateUnit<Distributor>;
			gen.StartGenerate (new Vector3 (0,0,0));
		}
	}
}
