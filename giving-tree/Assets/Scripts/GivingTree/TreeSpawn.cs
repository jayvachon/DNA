using UnityEngine;
using System.Collections;

public class TreeSpawn : MonoBehaviour {

	// big tree whose leaf we're a part of
	// set by Branch
	public GivingTree givingTree;

	// small tree on the leaf we're on
	// set by Leaf
	public Transform nextTree;

	public Transform Branch {
		get { return transform.parent; }		
	}

	public void OnIterate (Transform nextTree) {
		this.nextTree = nextTree;
	}
}
