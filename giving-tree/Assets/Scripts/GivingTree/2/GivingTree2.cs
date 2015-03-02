using UnityEngine;
using System.Collections;

public class GivingTree2 : MBRefs {

	public Transform givingTree;
	public Transform branch;
	public GameObject trunk;
	public float radius;
	public float height;

	const int targetTreeIndex = 9;
	public int TargetTreeIndex {
		get { return targetTreeIndex; }
	}

	Transform[] treeSpawns;
	public Transform[] TreeSpawns {
		get { return treeSpawns; }
	}

	public Transform TargetTree {
		get { return treeSpawns[targetTreeIndex]; }
	}

	GameObject[] branches;
	public GameObject[] Branches {
		get { return branches; }
		private set { branches = value; }
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
		branches = new GameObject[points.Length];
		for (int i = 0; i < points.Length; i ++) {
			Branch2 branch = CreateBranch (points[i]);
			branches[i] = branch.gameObject;
			treeSpawns[i] = branch.treeSpawn;
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
