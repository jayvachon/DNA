using UnityEngine;
using System.Collections;

public class DevelopmentPoint : PathPoint {

	public PathPointContainer container;

	void Start () {
		pointAction = new DevelopmentAction (3f, container);
		actionsList = new ActionsList (this, "Buildings");
	}

	public override void OnPerformAction (Action action) {
		switch (action.name) {
			case "Building": Debug.Log ("Building"); break;
			case "Hospital": Debug.Log ("Hospital"); break;
		}
	}
}

public class DevelopmentAction : PathPointAction {

	public PathPointContainer container;
	public DevelopmentAction (float time, PathPointContainer container) : base (time) {
		this.container = container;
	}

	public override void OnEndPerformAction () {
		container.ActivateDevelopInto ();
	}
}