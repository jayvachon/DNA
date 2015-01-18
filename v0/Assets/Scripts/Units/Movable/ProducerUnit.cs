using UnityEngine;
using System.Collections;
using GameActions;

public class ProducerUnit : MovableUnit, IActionable {

	public ActionsList ActionsList { get; set; }

	protected override void Awake () {
		base.Awake ();
		name = "Producer";
	}

	public void OnEndAction () {
		StartMoveOnPath ();
	}
}
