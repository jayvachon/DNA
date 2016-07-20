using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DNA.Units;

public class BuildingIndicator : FloatingIndicator {

	public Transform clinicRender;
	public Transform coffeeRender;
	public Transform jacuzziRender;
	public Transform milkshakePoolRender;
	public Transform universityRender;

	UnitRenderer visual;

	public static BuildingIndicator Instantiate (string id, Transform parent) {

		BuildingIndicator bi = ObjectPool.Instantiate<BuildingIndicator> ();
		bi.Init (id);
		bi.Initialize (parent, 2.25f);
		bi.StartSpinning ();
		
		return bi;
	}

	void Init (string id) {

		string renderer = UnitRenderer.GetRenderer (id);
		visual = ObjectPool.Instantiate (renderer) as UnitRenderer;
		visual.transform.parent = MyTransform;

		Vector3 s = visual.transform.localScale;
		visual.transform.localScale = new Vector3 (s.x*0.5f, s.y*0.5f, s.z*0.5f);
	}

	public override void OnEnable () {
		MyTransform.SetChildrenActive (false);
	}

	public override void OnDisable () {
		ObjectPool.Destroy (visual);
		MyTransform.SetChildrenActive (false);		
		base.OnDisable ();
	}
}
