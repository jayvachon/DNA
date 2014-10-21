using UnityEngine;
using System.Collections;

public class Cow : Unit {

	Transform iceCream;
	ObjectPool iceCreamPool;

	public override void OnStart () {
		renderer.SetColor (Color.white);
		CanSelect = false;
		iceCreamPool = ObjectPool.GetPool("IceCream");
	}

	void Update () {
		if (Input.GetKeyDown (KeyCode.A)) {
			Vector3 icPosition = MyTransform.position;
			icPosition.y += 1;
			iceCream = iceCreamPool.GetInstance (icPosition);
		}
	}
}
