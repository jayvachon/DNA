using UnityEngine;
using System.Collections;

public class PathGhost : MBRefs {

	void Move (float t, Vector3 from, Vector3 to) {
		Position = Vector3.Lerp (from, to, t);
	}
}
