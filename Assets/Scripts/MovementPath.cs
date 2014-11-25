using UnityEngine;
using System.Collections;

public class MovementPath {

	int stepSize = 10;
	bool ignoreY = true;

	public Vector3[] CreatePath (Vector3 start, Vector3 end) {

		if (ignoreY) {
			end.y = start.y;
		}

		float pathDistance = Vector3.Distance (start, end);
		int stepCount = Mathf.RoundToInt (pathDistance / stepSize);
		Vector3[] steps = new Vector3[stepCount];
		for (int i = 0; i < stepCount; i ++) {
			float pos = (float)i / (float)stepCount;
			steps[i] = Vector3.Lerp (start, end, pos);
		}
		return steps;
	}
}
