using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BuildingIndicator : FloatingIndicator {

	public Transform clinicRender;
	public Transform coffeeRender;
	public Transform jacuzziRender;
	public Transform milkshakePoolRender;
	public Transform universityRender;

	public static BuildingIndicator Instantiate (string id, Transform parent) {

		BuildingIndicator bi = ObjectPool.Instantiate<BuildingIndicator> ();
		bi.Init (id);
		bi.Initialize (parent, 1.5f);
		bi.StartSpinning ();
		
		return bi;
	}

	void Init (string id) {
		Transform activeRender = null;
		switch (id) {
			case "Clinic": activeRender = clinicRender; break;
			case "Coffee Plant": activeRender = coffeeRender; break;
			case "Jacuzzi": activeRender = jacuzziRender; break;
			case "Milkshake Derrick": activeRender = milkshakePoolRender; break;
			case "University": activeRender = universityRender; break;
		}
		activeRender.SetActiveRecursively (true);
	}

	public override void OnEnable () {
		MyTransform.SetChildrenActive (false);
	}

	public override void OnDisable () {
		MyTransform.SetChildrenActive (false);		
		base.OnDisable ();
	}
}
