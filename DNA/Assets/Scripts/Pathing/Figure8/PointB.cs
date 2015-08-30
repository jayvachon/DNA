using UnityEngine;
using System.Collections;

public class PointB : MonoBehaviour {

	void Update () {
		transform.RotateAround (Vector3.zero, Vector3.up, Time.deltaTime * 50f);
	}
}
