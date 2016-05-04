using UnityEngine;
using System.Collections;

public class SphereSeaPart : MBRefs {

	void OnEnable () {
		MyTransform.SetLocalScale (Random.Range (1.5f, 2f));
		MyTransform.localRotation = Random.rotation;
	}
}
