using UnityEngine;
using System.Collections;

// TODO: Make a class for HappinessIndicator and BuildingIndicator to inherit from
public class HappinessIndicator : MBRefs, IPoolable {

	[SerializeField] float spinSpeed = 15f;
	public Transform mercury;
	bool spinning = false;
	
	public float Fill {
		set { mercury.SetLocalScaleY (value); }
	}

	public void OnPoolCreate () {
		if (spinning) return;
		spinning = true;
		StartCoroutine (CoSpin ());
	}

	public void OnPoolDestroy () {
		spinning = false;
	}

	IEnumerator CoSpin () {
		
		float a = 0f;

		while (gameObject.activeSelf) {
			MyTransform.SetLocalEulerAnglesY (a);
			a += spinSpeed * Time.deltaTime;
			yield return null;
		}
	}
}
