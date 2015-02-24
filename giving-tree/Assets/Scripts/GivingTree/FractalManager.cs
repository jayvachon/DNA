using UnityEngine;
using System.Collections;

public class FractalManager : MonoBehaviour {

	class Iteration {

		Transform givingTreeTransform;
		GivingTree givingTree;
		Leaf[] leaves;

		Transform createdGivingTree;
		public Transform CreatedGivingTree {
			get {
				if (createdGivingTree == null) {
					createdGivingTree = givingTree.transform;
				}
				return createdGivingTree;
			}
		}

		public GivingTree GivingTree {
			get { return givingTree; }
		}

		public Leaf ActiveLeaf {
			get { return leaves [4]; }
		}

		public Transform ActiveLeafTransform {
			get { return leaves[4].treeSpawn; }
		}

		public Iteration (Transform givingTreeTransform, Transform parent, bool first=false) {
			this.givingTreeTransform = givingTreeTransform;
			Transform t = Instantiate (givingTreeTransform) as Transform;
			t.SetParent (parent);
			t.localPosition = Vector3.zero;
			this.givingTree = t.GetScript<GivingTree> ();
			this.givingTree.Create ();
			if (!first) t.localScale = new Vector3 (0.1f, 0.1f, 0.1f);
			leaves = this.givingTree.Leaves;
		}

		public Iteration[] Iterate () {
			Iteration[] iterations = new Iteration[leaves.Length];
			for (int i = 0; i < leaves.Length; i ++) {
				iterations[i] = new Iteration (givingTreeTransform, leaves[i].treeSpawn);
				leaves[i].OnIterate (iterations[i].GivingTree);
			}
			return iterations;
		}
	}

	public Transform givingTree;
	public Transform mainCamera;

	public TreeSpawn testSpawn;
	public Transform testTreeTransform;
	public GivingTree testTree;

	Iteration currentIteration;

	void Awake () {
		currentIteration = new Iteration (givingTree, transform, true);
		Iteration[] iterations = currentIteration.Iterate ();
		for (int i = 0; i < iterations.Length; i ++) {
			iterations[i].Iterate ();
		}
		mainCamera.SetParent (currentIteration.ActiveLeafTransform);
		mainCamera.localPosition = Vector3.zero;
	}

	void Update () {
		if (Input.GetKeyDown (KeyCode.Space)) {
			Zoom ();
		}
	}

	void Zoom () {
		testTree = currentIteration.ActiveLeaf.treeSpawn.GetScript<TreeSpawn> ().nextTree.GetScript<GivingTree> ();
		//Debug.Log (currentIteration.ActiveLeaf.treeSpawn.GetScript<TreeSpawn> ().nextTree.GetScript<GivingTree> ().Leaves.Length);
		//mainCamera.SetParent (currentIteration.ActiveLeaf.treeSpawn.GetScript<TreeSpawn> ().nextTree.GetScript<GivingTree> ().Leaves[4].treeSpawn); // lol
		mainCamera.SetParent (currentIteration.ActiveLeaf.treeSpawn.GetScript<TreeSpawn> ().nextTree);
		StartCoroutine (CoZoom ());
	}

	IEnumerator CoZoom () {
		float time = 1;
		float eTime = 0;
		Transform t = currentIteration.CreatedGivingTree;
		Vector3 targetScale = new Vector3 (10, 10, 10);

		while (eTime < time) {
			eTime += Time.deltaTime;
			t.localScale = Vector3.Lerp (Vector3.one, targetScale, Mathf.Lerp (0, 1, eTime / time));
			yield return null;
		}

		eTime = 0;
		while (eTime < time) {
			eTime += Time.deltaTime;
			mainCamera.localScale = Vector3.Lerp (targetScale, Vector3.one, Mathf.Lerp (0, 1, eTime / time));
			mainCamera.localPosition = Vector3.zero;
			yield return null;
		}
	}
}
