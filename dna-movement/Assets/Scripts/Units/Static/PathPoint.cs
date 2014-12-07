using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PathPoint : StaticUnit {

	protected PathPointAction pointAction = null;
	
	bool activated = true;
	public bool Activated {
		get { return activated; }
		set {
			if (value) {
				renderer.enabled = true;
				activated = true;
			} else {
				renderer.enabled = false;
				activated = false;
			}
		}
	}

	public void ArriveAtPoint (MovableUnit u) {
		if (pointAction == null)
			u.OnEndPathAction ();
		else
			StartCoroutine (PerformAction (u));
	}

	IEnumerator PerformAction (MovableUnit u) {
		
		MovableUnit callingUnit = u;
		float time = pointAction.time;
		float eTime = 0f;

		while (eTime < time) {
			eTime += Time.deltaTime;
			pointAction.PerformAction (eTime / time);
			yield return null;
		}

		pointAction.OnEndPerformAction ();
		callingUnit.OnEndPathAction ();
	}

	public override void OnUnitClickEvent (UnitClickEvent e) {}
	public override void OnFloorClickEvent (FloorClickEvent e) {}
}

public class PathPointAction {

	public readonly float time;

	public PathPointAction (float time) {
		this.time = time;
	}

	// This function gets called every frame while the unit is at this point
	public virtual void PerformAction (float percent) {}

	// Gets called at the end of the time
	public virtual void OnEndPerformAction () {}
}
