using UnityEngine;
using System.Collections;

public class BuildUnitAction : Action, IGUIActionable {

	public readonly string name;
	public string Label {
		get { return name; }
	}

	public BuildUnitAction (float time, string name) : base (time) {
		this.name = name;
	}
}
