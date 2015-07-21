using UnityEngine;
using System.Collections;

public class PlotsCreator2 : MonoBehaviour {

	public Transform plot;
	[Range (0.1f, 10f)] public float radius = 0.1f;
	[Range (0f, 1f)] public float altitude = 0.033f;

	Transform[] plots;
	Fermat fermat = new Fermat ();
	int pointCount;

	void Awake () {
		pointCount = fermat.Points.Length;
		plots = new Transform[pointCount];
		SetPointPositions ();
	}

	void Update () {
		fermat.UpdateSettings (
			new Fermat.Settings (radius, pointCount, altitude));
		SetPointPositions ();
	}

	void SetPointPositions () {
		Vector3[] points = fermat.Points;
		for (int i = 0; i < points.Length; i ++) {
			if (plots[i] == null) plots[i] = Instantiate (plot) as Transform;
			plots[i].position = points[i];
			plots[i].SetParent (transform);
		}
	}
}
