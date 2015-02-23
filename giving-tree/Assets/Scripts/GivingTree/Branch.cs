using UnityEngine;
using System.Collections;

public class Branch : MonoBehaviour {

	public Transform branchRender;
	public Transform leafTransform;
	public float length;
	public float radius;

	Leaf leaf = null;
	public Leaf Leaf {
		get {
			if (leaf == null) {
				leaf = leafTransform.GetScript<Leaf> ();
			}
			return leaf;
		}
	}

	void Awake () {
		branchRender.SetPositionZ (length * 0.5f);
		branchRender.localScale = new Vector3 (
			radius,
			length * 0.5f,
			radius
		);
		leafTransform.SetLocalPositionZ (length);
	}
}
