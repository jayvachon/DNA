using UnityEngine;
using System.Collections;

public class Iteration3 : MonoBehaviour {

	public Transform treePrefab;

	const int targetTreeIndex = 9;
	bool iterated = false;
	GivingTree2[] trees;

	GivingTree2 thisTree;
	public GivingTree2 ThisTree {
		get { return thisTree; }
	}

	GivingTree2 targetTree;
	public GivingTree2 TargetTree {
		get { return targetTree; }
		private set { targetTree = value; }
	}

	public Transform TargetTreeTransform {
		get { return TargetTree.transform; }
	}

	public void Create () {
		Transform t = Instantiate (treePrefab) as Transform;
		thisTree = t.GetScript<GivingTree2> ();
		Iterate ();
		TargetTree = trees[targetTreeIndex];
	}

	void Iterate () {
		if (iterated) return;
		iterated = true;
		Transform[] spawns = thisTree.TreeSpawns;
		trees = new GivingTree2[spawns.Length];
		for (int i = 0; i < spawns.Length; i ++) {
			trees[i] = CreateTree (spawns[i].position);
		}
	}

	GivingTree2 CreateTree (Vector3 position) {
		Transform t = Instantiate (treePrefab, position, Quaternion.identity) as Transform;
		t.SetParent (thisTree.transform);
		GivingTree2 tree = t.GetScript<GivingTree2> ();
		t.SetLocalScale (0.1f);
		return tree;
	}
}
