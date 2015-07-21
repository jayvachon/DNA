using UnityEngine;
using System.Collections;

public class GivingTree : MonoBehaviour {

	public Transform plot;

	FermatsSpiral spiral;

	void Awake () {
		spiral = new FermatsSpiral ();
		Vector3[] positions = spiral.Positions;
		Vector3[] rotations = spiral.Rotations;
		for (int i = 0; i < positions.Length; i ++) {
			Transform newPlot = Instantiate (plot) as Transform;
			newPlot.position = spiral.Positions[i];
			float scale = Mathf.Lerp (1f, 0f, (float)i / (float)positions.Length);
			newPlot.localScale = new Vector3 (scale, 0.1f * scale, scale);
			newPlot.localEulerAngles = rotations[i];
			newPlot.SetParent (transform);
		}
	}

	void Update () {
		spiral.Draw ();
	}
}
