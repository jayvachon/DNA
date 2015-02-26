using UnityEngine;
using System.Collections;

public class Iteration : MonoBehaviour {

	public Transform prefab;

	int targetTreeIndex = 9;
	bool iterated = false;
	GivingTree2[] trees;

	GivingTree2 givingTree;
	public GivingTree2 ThisTree {
		get { return givingTree; }
	}

	GivingTree2 targetTree;
	public GivingTree2 TargetTree {
		get { return targetTree; }
		private set { targetTree = value; }
	}

	public Transform TargetTransform {
		get { return TargetTree.transform; }
	}

	public void Create (GivingTree2 givingTree) {
		this.givingTree = givingTree;
		Iterate (givingTree);
		TargetTree = trees[targetTreeIndex];
	}

	void Iterate (GivingTree2 parent) {
		if (iterated) {
			return;
		}
		iterated = true;
		Transform[] spawns = parent.TreeSpawns;
		trees = new GivingTree2[spawns.Length];
		for (int i = 0; i < spawns.Length; i ++) {
			trees[i] = CreateTree (spawns[i].position, parent.transform);
		}
	}

	GivingTree2 CreateTree (Vector3 position, Transform parent) {
		Transform t = Instantiate (prefab, position, Quaternion.identity) as Transform;
		t.SetParent (parent);
		GivingTree2 tree = t.GetScript<GivingTree2> ();
		t.SetLocalScale (0.1f);
		return tree;
	}

	public void DeactivateUntargeted () {
		GameObject[] branches = givingTree.Branches;
		for (int i = 0; i < trees.Length; i ++) {
			if (trees[i] != TargetTree) {
				branches[i].SetActive (false);
				trees[i].gameObject.SetActive (false);
			}
		}
	}
}
