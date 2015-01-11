using UnityEngine;
using System.Collections;

public class GenerateIceCreamAction : Action, IGUIActionable {

	public string Label {
		get { return "Generate Ice Cream"; }
	}

	public GenerateIceCreamAction (float time=3f) : base (time) {

	}

	public override void OnEndAction (IActionable point, IActionable visitor) {
		base.OnEndAction (point, visitor);
	}
}
