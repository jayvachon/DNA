using UnityEngine;
using System.Collections;
using GameActions;

public class ProducerUnit : MovableUnit, IActionable {

	public override string Name {
		get { return "Producer"; }
	}
	
	public ActionsList ActionsList { get; set; }

	protected override void Awake () {
		base.Awake ();
	}

	public void OnEndAction () {
		StartMoveOnPath ();
	}
}
