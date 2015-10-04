using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BuildingIndicator : FloatingIndicator {

	public Transform clinicRender;
	public Transform coffeeRender;
	public Transform jacuzziRender;
	public Transform milkshakePoolRender;
	public Transform universityRender;

	public void Initialize (string id, Transform parent) {
		Transform activeRender = null;
		switch (id) {
			case "Clinic": activeRender = clinicRender; break;
			case "Coffee Plant": activeRender = coffeeRender; break;
			case "Jacuzzi": activeRender = jacuzziRender; break;
			case "Milkshake Derrick": activeRender = milkshakePoolRender; break;
			case "University": activeRender = universityRender; break;
		}
		activeRender.SetActiveRecursively (true);
		Initialize (parent, 1.5f);
		StartSpinning ();
	}

	public override void OnEnable () {
		MyTransform.SetChildrenActive (false);
	}

	public override void OnDisable () {
		MyTransform.SetChildrenActive (false);		
		base.OnDisable ();
	}
}
