using UnityEngine;
using System.Collections;

public class GivingTree : MonoBehaviour {

	public Transform plot;

	Helix helix;

	void Awake () {
		helix = new Helix ();
		Vector3[] positions = helix.Positions;
		Vector3[] rotations = helix.Rotations;
		for (int i = 0; i < positions.Length; i ++) {
			Transform newPlot = Instantiate (plot) as Transform;
			newPlot.position = helix.Positions[i];
			float scale = Mathf.Lerp (1f, 0f, (float)i / (float)positions.Length);
			newPlot.localScale = new Vector3 (scale, 0.1f * scale, scale);
			newPlot.localEulerAngles = rotations[i];
			newPlot.SetParent (transform);
		}
	}

	void Update () {
		helix.Draw ();
	}
}
