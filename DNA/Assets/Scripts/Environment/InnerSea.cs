using UnityEngine;
using System.Collections;

public class InnerSea : MBRefs {

	const float minLevel = -5f;
	const float maxLevel = 10f;

	void Awake () {
		MyTransform.SetLocalPositionY (minLevel);
		StartCoroutine (CoRise ());
	}

	IEnumerator CoRise () {
		
		float time = 7f;
		float eTime = 0f;
	
		while (eTime < time) {
			eTime += Time.deltaTime;
			float progress = Mathf.SmoothStep (0, 1, eTime / time);
			MyTransform.SetLocalPositionY (Mathf.Lerp (minLevel, maxLevel, progress));
			yield return null;
		}

		StartCoroutine (CoRise ());
	}
}
