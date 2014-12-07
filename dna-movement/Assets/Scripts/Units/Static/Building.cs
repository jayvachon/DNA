using UnityEngine;
using System.Collections;

public class Building : PathPoint {

	void Start () {
		pointAction = new BuildAction (1f);
		renderer.SetColor (Color.blue);
	}
}

public class BuildAction : PathPointAction {

	public BuildAction (float time) : base (time) {}

	public override void PerformAction (float percent) {

	}
}
