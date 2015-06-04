using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BuildingIndicator : FloatingIndicator, IPoolable {

	public Transform clinicRender;
	public Transform coffeeRender;
	public Transform jacuzziRender;
	public Transform milkshakePoolRender;

	public void Initialize (string id, Transform parent) {
		Transform activeRender = null;
		switch (id) {
			case "Clinic": activeRender = clinicRender; break;
			case "Coffee Plant": activeRender = coffeeRender; break;
			case "Jacuzzi": activeRender = jacuzziRender; break;
			case "Milkshake Derrick": activeRender = milkshakePoolRender; break;
		}
		activeRender.SetActiveRecursively (true);
		Initialize (parent, 1.5f);
	}

	public override void OnPoolCreate () {
		base.OnPoolCreate ();
		MyTransform.SetChildrenActive (false);
	}
}
