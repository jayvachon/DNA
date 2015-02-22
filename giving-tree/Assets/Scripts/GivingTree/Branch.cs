using UnityEngine;
using System.Collections;

public class Branch : MonoBehaviour {

	public Transform branchRender;
	public Transform leaf;
	public float length;
	public float radius;

	void Awake () {
		branchRender.SetPositionZ (length * 0.5f);
		branchRender.localScale = new Vector3 (
			radius,
			length * 0.5f,
			radius
		);
		leaf.SetLocalPositionZ (length);
	}
}
