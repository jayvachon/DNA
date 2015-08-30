using UnityEngine;
using System.Collections;

public class Cow : Unit {

	ObjectPool iceCreamPool;

	public override void OnStart () {
		Init (Color.black, Color.black);
		CanSelect = false;
		iceCreamPool = ObjectPool.GetPool("IceCream");
		Invoke ("CreateIceCream", 5f + Random.Range (1f, 5f));
	}

	void CreateIceCream () {
		Vector3 icPosition = MyTransform.position;
		icPosition.y += 1;
		iceCreamPool.GetInstance (icPosition);
		Invoke ("CreateIceCream", 5f + Random.Range (1f, 5f));
	}
}
