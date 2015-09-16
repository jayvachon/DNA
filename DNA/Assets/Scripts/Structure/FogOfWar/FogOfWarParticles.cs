using UnityEngine;
using System.Collections;

public class FogOfWarParticles : MBRefs, IPoolable {

	void Start () {
		MyTransform.LookAt (new Vector3 (0, -28.7f, 0), Vector3.up);
		MyTransform.SetLocalEulerAnglesX (MyTransform.localEulerAngles.x - 90f);
	}

	void Update () {
		transform.rotation = Camera.main.transform.rotation;
	}

	public void OnPoolCreate () {}
	public void OnPoolDestroy () {}
}
