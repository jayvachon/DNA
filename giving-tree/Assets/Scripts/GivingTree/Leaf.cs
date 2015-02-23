using UnityEngine;
using System.Collections;

public class Leaf : MonoBehaviour {

	public Transform givingTree;
	public Transform leafRender;
	public float width;
	public float height;
	public float depth;

	void Awake () {
		leafRender.SetLocalPositionZ (height * 0.5f);
		leafRender.localScale = new Vector3 (width, height, depth);
	}
}
