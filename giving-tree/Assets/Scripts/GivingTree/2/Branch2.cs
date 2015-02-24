using UnityEngine;
using System.Collections;

public class Branch2 : MBRefs {

	public Transform branchBottom;
	public Transform branchSide;
	public Transform leaf;
	public Transform treeSpawn;
	public float length;

	protected override void Awake () {
		base.Awake ();
		branchBottom.SetLocalPositionZ (length * 0.5f);
		branchBottom.SetLocalScaleY (length * 0.5f);
		branchSide.SetLocalPositionZ (length);
		leaf.SetLocalPositionZ (length);
		treeSpawn.SetLocalPositionZ (length);
	}
}
