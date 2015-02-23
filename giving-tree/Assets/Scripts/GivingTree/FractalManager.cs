using UnityEngine;
using System.Collections;

public class FractalManager : MonoBehaviour {

	class Iteration {

		Transform givingTreeTransform;
		GivingTree givingTree;
		Leaf[] leaves;

		public Iteration (Transform givingTreeTransform, Vector3 position, float scale) {
			this.givingTreeTransform = givingTreeTransform;
			Transform t = Instantiate (givingTreeTransform) as Transform;
			t.position = position;
			this.givingTree = t.GetScript<GivingTree> ();
			this.givingTree.Create ();
			t.localScale = new Vector3 (scale, scale, scale);
			leaves = this.givingTree.Leaves;
		}

		public Iteration[] Iterate () {
			Iteration[] iterations = new Iteration[leaves.Length];
			float scale = givingTree.transform.localScale.x;
			for (int i = 0; i < leaves.Length; i ++) {
				iterations[i] = new Iteration (givingTreeTransform, leaves[i].transform.position, scale * 0.1f);
			}
			return iterations;
		}
	}

	public Transform givingTree;

	int iteration = 0;

	void Awake () {
		Iteration iteration = new Iteration (givingTree, Vector3.zero, 1);
		Iteration[] iterations = iteration.Iterate ();
		for (int i = 0; i < iterations.Length; i ++) {
			Iteration[] iterations2 = iterations[i].Iterate ();
			for (int j = 0; j < iterations2.Length; j ++) {
				iterations2[i].Iterate ();
			}
		}
	}
}
