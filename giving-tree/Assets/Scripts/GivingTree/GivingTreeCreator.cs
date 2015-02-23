using UnityEngine;
using System.Collections;

public class GivingTreeCreator : MonoBehaviour {

	public static bool createTree = true;
	public Transform givingTree;

	void Awake () {
		Transform tree = Instantiate (givingTree) as Transform;
		tree.GetScript<GivingTree> ().Create ();
	}
}
