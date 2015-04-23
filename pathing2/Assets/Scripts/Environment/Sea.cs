using UnityEngine;
using System.Collections;

public delegate void EndRising ();

public class Sea : MBRefs {

	public EndRising endRising;

	float timescale = 600f; 
	float rate = 1;

	const float minLevel = -3.5f;
	const float maxLevel = 0.1f;

	float level;
	float Level {
		get { return level; }
		set {
			level = Mathf.Lerp (minLevel, maxLevel, value);
			MyTransform.SetLocalPositionY (level);
		}
	}

	public void BeginRising () {
		Level = 0;
		StartCoroutine (CoRise ());
	}

	IEnumerator CoRise () {
		
		float time = timescale;
		float eTime = 0f;
	
		while (eTime < time) {
			eTime += Time.deltaTime * rate;
			Level = Mathf.Lerp (0f, 1f, eTime / time);
			yield return null;
		}

		if (endRising != null) endRising ();
	}

	void Update () {
		if (Input.GetKeyDown (KeyCode.Q)) {
			rate += 0.1f;
			Debug.Log (rate);
		}
		if (Input.GetKeyDown (KeyCode.W)) {
			rate -= 0.1f;
			Debug.Log (rate);
		}
	}
}
