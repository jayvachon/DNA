using UnityEngine;
using System.Collections;

public class Iteration : MonoBehaviour {

	// needs to know:
	// where to create trees on THIS tree (treeSpawns)
	// where to create trees on EACH LEAF (treeSpawns)
	// which leaf we're on
	// which leaf we're going to

	public Transform prefab;

	GivingTree2[] trees;
	int targetTreeIndex = 6;

	GivingTree2 givingTree;
	public GivingTree2 ThisTree {
		get { return givingTree; }
	}

	public GivingTree2 TargetTree {
		get { return trees[targetTreeIndex]; }
	}

	public void Create (GivingTree2 givingTree) {
		this.givingTree = givingTree;
		trees = Iterate (givingTree);
	}

	GivingTree2[] Iterate (GivingTree2 parent) {
		Transform[] spawns = parent.TreeSpawns;
		GivingTree2[] trees = new GivingTree2[spawns.Length];
		for (int i = 0; i < spawns.Length; i ++) {
			trees[i] = CreateTree (spawns[i].position, parent.transform);
		}
		return trees;
	}

	GivingTree2 CreateTree (Vector3 position, Transform parent) {
		Transform t = Instantiate (prefab, position, Quaternion.identity) as Transform;
		t.SetParent (parent);
		GivingTree2 tree = t.GetScript<GivingTree2> ();
		t.SetLocalScale (0.1f);
		return tree;
	}
}
