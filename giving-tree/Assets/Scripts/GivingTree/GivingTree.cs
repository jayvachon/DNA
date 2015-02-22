using UnityEngine;
using System.Collections;

public class GivingTree : MBRefs {

	public Transform branch;
	public Trunk trunk;

	public void Create (int iteration) {
		CreateLeaves ();
	}

	void CreateLeaves () {
		Vector4[] points = trunk.Helix.Points;
		for (int i = 0; i < points.Length; i ++) {
			Vector4 point = points[i];
			Vector3 position = new Vector3 (point.x, point.y, point.z);
			Transform branchTransform = Instantiate (branch) as Transform;
			branchTransform.SetPosition (position);
			branchTransform.SetLocalEulerAnglesY (point.w);
			branchTransform.SetParent (MyTransform);
		}
	}

	/**
	 *	Debugging
	 */	

	void Update () {
		trunk.Helix.Draw ();
	}
}
