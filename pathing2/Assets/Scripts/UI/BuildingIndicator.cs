using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BuildingIndicator : MBRefs, IPoolable {

	[SerializeField] float spinSpeed = 10f;
	public Transform clinicRender;
	public Transform coffeeRender;
	public Transform jacuzziRender;
	public Transform milkshakePoolRender;

	public void Initialize (string id, Vector3 position) {
		Transform activeRender = null;
		Debug.Log (id);
		switch (id) {
			case "Clinic": activeRender = clinicRender; break;
			case "Coffee Plant": activeRender = coffeeRender; break;
			case "Jacuzzi": activeRender = jacuzziRender; break;
			case "Milkshake Derrick": activeRender = milkshakePoolRender; break;
		}
		activeRender.SetActiveRecursively (true);
		
		Position = position;
		MyTransform.SetLocalPositionY (1.5f);
		StartCoroutine (CoSpin ());
	}

	IEnumerator CoSpin () {
		
		float a = 0f;

		while (gameObject.activeSelf) {
			MyTransform.SetLocalEulerAnglesY (a);
			a += spinSpeed * Time.deltaTime;
			yield return null;
		}
	}

	public void OnPoolCreate () {
		MyTransform.SetChildrenActive (false);
	}

	public void OnPoolDestroy () {}
}
