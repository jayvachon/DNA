using UnityEngine;
using System.Collections;

public class PointRotationMB : MonoBehaviour {

	Vector3 point = Vector3.zero;

	void Update () {
		GizmosDrawer.Instance.Clear ();
		DrawClock ();
	}

	void DrawClock () {
		GizmosDrawer.Instance.Add (new GizmoSphere (point, 0.1f));
	}

}
