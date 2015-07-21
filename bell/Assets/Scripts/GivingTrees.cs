using UnityEngine;
using System.Collections;

public class GivingTrees : MonoBehaviour {

	public Transform givingTree;

	FermatsSpiral spiral;

	void Awake () {
		spiral = new FermatsSpiral (10);
		Vector3[] positions = spiral.Positions;
		Vector3[] rotations = spiral.Rotations;
		for (int i = 0; i < positions.Length; i ++) {
			Transform newPlot = Instantiate (givingTree) as Transform;
			newPlot.position = spiral.Positions[i];
			float scale = Mathf.Lerp (0.1f, 0f, (float)i / (float)positions.Length);
			newPlot.localScale = new Vector3 (scale, scale, scale);
			newPlot.localEulerAngles = rotations[i];
			newPlot.SetParent (transform);
		}
	}
}
