using UnityEngine;
using System.Collections;

public class GivingTree : MBRefs {

	public Transform branch;
	public Trunk trunk;

	Leaf[] leaves;
	public Leaf[] Leaves {
		get { return leaves; }
	}

	public void Create () {
		CreateLeaves ();
	}

	void CreateLeaves () {
		Vector4[] points = trunk.Helix.Points;
		leaves = new Leaf[points.Length];
		for (int i = 0; i < points.Length; i ++) {
			Vector4 point = points[i];
			Vector3 position = new Vector3 (point.x, point.y, point.z);
			Transform branchTransform = Instantiate (branch) as Transform;
			branchTransform.SetParent (MyTransform);
			branchTransform.SetLocalPosition (position);
			branchTransform.SetLocalEulerAnglesY (point.w);
			leaves[i] = branchTransform.GetScript<Branch> ().Leaf;
		}
	}

	/**
	 *	Debugging
	 */	

	void Update () {
		trunk.Helix.Draw ();
	}
}
