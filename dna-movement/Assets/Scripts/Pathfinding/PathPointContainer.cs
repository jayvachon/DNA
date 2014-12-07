using UnityEngine;
using System.Collections;

public class PathPointContainer : StaticUnit {

	public Building building;

	public DevelopmentPoint devPoint;
	PathPoint developInto = null;
	PathPoint activePoint;

	bool activated = false;
	public bool Activated {
		get { return activated; }
	}

	void Start () {
		activePoint = devPoint;
	}

	public void ArriveAtPoint (MovableUnit u) {
		activePoint.ArriveAtPoint (u);
	}

	public void SetDevelopIntoBuilding () {
		if (developInto != null)
			return;
		developInto = GameObject.Instantiate (building, StartPosition, Quaternion.identity) as PathPoint;
		developInto.MyTransform.SetParent (MyTransform);
		developInto.Activated = false;
		activated = true;
	}

	public void ActivateDevelopInto () {
		devPoint.Activated = false;
		activePoint = developInto;
		activePoint.Activated = true;
	}

	public override void OnSelect () {
		activePoint.Select ();
	}

	public override void OnUnselect () {
		activePoint.Unselect ();
	}

	/**
	*	Debugging
	*/

	void Update () {
		if (!Selected)
			return;
		if (Input.GetKeyDown (KeyCode.Space)) {
			SetDevelopIntoBuilding ();
		}
	}
}
