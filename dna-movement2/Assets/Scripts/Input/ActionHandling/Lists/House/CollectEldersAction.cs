using UnityEngine;
using System.Collections;

public class CollectEldersAction : Action, IGUIActionable {

	public string Label {
		get { return "Collect Elders"; }
	}

	public CollectEldersAction (float time=1f) : base (time) {}

	public override void OnEndAction (IActionable point, IActionable visitor) {
		base.OnEndAction (point, visitor);
	}
}
