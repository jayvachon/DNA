using UnityEngine;
using System.Collections;

public class GivingTrees : MonoBehaviour {

	public Transform givingTree;

	Helix helix;

	void Awake () {
		helix = new Helix (10);
		Vector3[] positions = helix.Positions;
		Vector3[] rotations = helix.Rotations;
		for (int i = 0; i < positions.Length; i ++) {
			Transform newPlot = Instantiate (givingTree) as Transform;
			newPlot.position = helix.Positions[i];
			float scale = Mathf.Lerp (0.1f, 0f, (float)i / (float)positions.Length);
			newPlot.localScale = new Vector3 (scale, scale, scale);
			newPlot.localEulerAngles = rotations[i];
			newPlot.SetParent (transform);
		}
	}
}
