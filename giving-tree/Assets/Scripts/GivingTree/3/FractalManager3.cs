using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FractalManager3 : MonoBehaviour {

	public Transform cameraAnchor;
	public GivingTree2 treePrefab;

	int maxIterations = 3;
	List<GivingTree2> visitableTrees = new List<GivingTree2> ();

	GivingTree2 LastTree {
		get { return visitableTrees[visitableTrees.Count-1]; }
	}

	int treeCount = 0;

	void Awake () {
		GivingTree2 targetTree = CreateTree (Vector3.zero, transform, true);
		for (int i = 1; i < maxIterations; i ++) {
			targetTree = CreateTree (targetTree.TargetTree.position, targetTree.transform, true);
		}
	}

	GivingTree2 CreateTree (Vector3 position, Transform parent, bool visitable, GivingTree2 tree=null) {
		
		if (tree == null) {
			tree = Instantiate (treePrefab) as GivingTree2;
		}

		Transform treeTransform = tree.transform;
		treeTransform.SetLocalPosition (position);
		treeTransform.SetParent (parent);
		treeTransform.SetLocalScale (0.1f);
		
		if (visitable) {
			tree.name = string.Format ("Tree {0}", treeCount);
			treeCount ++;
			visitableTrees.Add (tree);
		}

		return tree;
	}

	void Iterate () {
		GivingTree2 next = visitableTrees[0];
		visitableTrees.Remove (next);
		visitableTrees[0].transform.SetParent (transform);
		visitableTrees[0].transform.SetLocalScale (0.1f);
		visitableTrees[0].transform.SetLocalPosition (Vector3.zero);
		CreateTree (LastTree.TargetTree.position, LastTree.transform, true, next);
	}

	void DebugTrees () {
		for (int i = 0; i < visitableTrees.Count; i ++) {
			Debug.Log (visitableTrees[i].name);
		}
	}

	void Update () {
		if (Input.GetKeyDown (KeyCode.UpArrow)) {
			Iterate ();
		}
	}
}
