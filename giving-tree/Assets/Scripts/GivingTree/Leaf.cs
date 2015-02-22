using UnityEngine;
using System.Collections;

public class Leaf : MonoBehaviour {

	public Transform givingTree;
	public Transform leafRender;
	public float width;
	public float height;

	void Awake () {
		leafRender.SetLocalPositionZ (height * 0.5f);
		leafRender.localScale = new Vector3 (
			width,
			height,
			1
		);
		if (GivingTreeCreator.createTree) {
			//Transform tree = Instantiate (givingTree, transform.position, Quaternion.identity) as Transform;
			//tree.GetScript<GivingTree> ().Create (1);
			GivingTreeCreator.createTree = false;
		}
	}
}
