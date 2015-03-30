using UnityEngine;
using System.Collections;

public class Pivot : MonoBehaviour {

	void Update () {
		transform.Rotate (Vector3.up * 10f * Time.deltaTime);
	}
}
