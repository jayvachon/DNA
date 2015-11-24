using UnityEngine;
using System.Collections;

public class PathMovementPoint : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        transform.rotation = Quaternion.FromToRotation(Vector3.up, transform.forward);
	}
}
