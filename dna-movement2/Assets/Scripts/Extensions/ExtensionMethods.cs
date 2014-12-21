using UnityEngine;
using System.Collections;

public static class ExtensionMethods {
	
	public static Vector3 NullPosition = new Vector3 (-1, -1, -1);

	public static void SetVertexPositions (this LineRenderer lineRenderer, Vector3[] positions) {
		lineRenderer.SetVertexCount(positions.Length);
		for (int i = 0; i < positions.Length; i ++) {
			lineRenderer.SetPosition (i, positions[i]);
		}
	}

}
