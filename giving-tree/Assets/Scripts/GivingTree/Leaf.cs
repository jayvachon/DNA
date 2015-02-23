using UnityEngine;
using System.Collections;

public class Leaf : MonoBehaviour {

	public Transform givingTree;
	public Transform leafRender;
	public Transform treeSpawn;
	public float width;
	public float height;
	public float depth;

	void Awake () {
		leafRender.SetLocalPositionZ (height * 0.5f);
		leafRender.localScale = new Vector3 (width, height, depth);
		treeSpawn.localPosition = new Vector3 (0, depth * 0.5f, height * 0.5f);
	}

	public void OnIterate (GivingTree nextTree) {
		treeSpawn.GetScript<TreeSpawn> ().OnIterate (nextTree.Leaves[4].treeSpawn);
	}
}
