using UnityEngine;
using System.Collections;

public class FractalManager2 : MonoBehaviour {
		
	public Transform givingTree;
	int iteration = 0;

	void Awake () {
		Transform t = Instantiate (givingTree) as Transform;
		GivingTree2 tree = t.GetScript<GivingTree2> ();
		tree.Create (iteration);
		tree.Iterate ();
	}
}
