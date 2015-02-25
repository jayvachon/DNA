using UnityEngine;
using System.Collections;

public class FractalManager2 : MonoBehaviour {

	public Transform cameraAnchor;
	public Iteration iteration;
	public Transform givingTree;
	GivingTree2 initialTree;
	GivingTree2[] nextTrees;

	int target = 6;

	public Transform CurrentLeaf {
		get { return initialTree.TreeSpawns[target].transform; }
	}

	void Awake () {
		initialTree = CreateInitialTree ();
		cameraAnchor.SetParent (CurrentLeaf);
		cameraAnchor.SetLocalPosition (new Vector3 (0, 0, 0));

		GivingTree2 targetTree = initialTree;
		for (int i = 0; i < 5; i ++) {
			targetTree.name = string.Format ("Tree #{0}", i);
			targetTree = Iterate (targetTree);
		}
	}
	
	GivingTree2 CreateInitialTree () {
		Transform t = Instantiate (givingTree) as Transform;
		t.SetParent (transform);
		GivingTree2 tree = t.GetScript<GivingTree2> ();
		return tree;
	}

	GivingTree2 Iterate (GivingTree2 tree) {
		Iteration tempIteration = Instantiate (iteration) as Iteration;
		tempIteration.Create (tree);
		return tempIteration.TargetTree;
	}

	void Update () {
		if (Input.GetKeyDown (KeyCode.UpArrow)) {

			StartCoroutine (Expand ());
		}
	}

	IEnumerator Expand () {
		Transform tree = initialTree.transform;
		float time = 3;
		float eTime = 0;
		while (eTime < time) {
			eTime += Time.deltaTime;
			tree.SetLocalScale (Mathf.Lerp (1, 10, eTime / time));
			yield return null;
		}
	}
}
