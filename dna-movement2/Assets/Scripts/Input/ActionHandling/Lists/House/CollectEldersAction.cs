using UnityEngine;
using System.Collections;

public class CollectEldersAction : Action, IGUIActionable {

	public string Label {
		get { return "Collect Elders"; }
	}

	public CollectEldersAction (float time=1f) : base (time) {}

	public override void OnEndAction (IActionable point, IActionable visitor) {
		
		base.OnEndAction (point, visitor);
		IElderHoldable holdable1 = point as IElderHoldable;
		IElderHoldable holdable2 = visitor as IElderHoldable;
		
		/*int elders = holdable1.Elders.Clear ();
		int overflow = holdable2.Elders.Add (elders);
		holdable1.Elders.Add (overflow);*/
		ElderHolder.Trade (holdable1, holdable2);
	}
}
