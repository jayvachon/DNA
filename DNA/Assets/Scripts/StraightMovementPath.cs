using UnityEngine;
using System.Collections;

public class StraightMovementPath : MovementPath {

	// Path attributes
	float distance;

	public StraightMovementPath (Vector3 start, Vector3 end) {
		
		if (ignoreY) {
			end.y = start.y;
		}
		
		distance = Vector3.Distance (start, end);
		CreatePath (start, end);
	}

	void CreatePath (Vector3 start, Vector3 end) {

		int stepCount = Mathf.Max (1, Mathf.RoundToInt (distance / stepSize));
		path = new Vector3[stepCount+1];
		
		for (int i = 0; i < stepCount; i ++) {
			float pos = (float)i / (float)stepCount;
			path[i] = Vector3.Lerp (start, end, pos);
		}

		path[path.Length-1] = end;
	}
}