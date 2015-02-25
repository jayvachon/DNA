using UnityEngine;
using System.Collections;

public class GivingTree2 : MBRefs {

	public Transform givingTree;
	public Transform branch;
	public float radius;
	public float height;
	public bool iterate = false;

	Transform[] treeSpawns;
	public Transform[] TreeSpawns {
		get { return treeSpawns; }
	}

	Helix helix;

	protected override void Awake () {
		base.Awake ();
		helix = new Helix (radius, height);
		CreateBranches ();
	}

	void CreateBranches () {
		Vector4[] points = helix.Points;
		treeSpawns = new Transform[points.Length];
		for (int i = 0; i < points.Length; i ++) {
			treeSpawns[i] = CreateBranch (points[i]).treeSpawn;
		}
	}

	Branch2 CreateBranch (Vector4 point) {
		Vector3 position = new Vector3 (point.x, point.y, point.z);
		Transform newBranch = Instantiate (branch) as Transform;
		newBranch.SetParent (MyTransform);
		newBranch.SetLocalPosition (position);
		newBranch.SetLocalEulerAnglesY (point.w);
		return newBranch.GetScript<Branch2> ();
	}
	
	void CreateTree (Vector3 position) {
		Transform newTree = Instantiate (givingTree, position, Quaternion.identity) as Transform;
		newTree.SetParent (MyTransform);
		newTree.SetLocalScale (0.1f);
	}
}
