using UnityEngine;
using System.Collections;

public delegate void EndRising ();

public class Sea : MBRefs {

	static Sea instance = null;
	static public Sea Instance {
		get {
			if (instance == null) {
				instance = Object.FindObjectOfType (typeof (Sea)) as Sea;
			}
			return instance;
		}
	}

	const float minLevel = -10f;
	const float maxLevel = 5f;
	public EndRising endRising;
	float timescale = 600f; 

	float rate = 0;
	public float Rate {
		get { return rate; }
		set { rate = value; }
	}

	float level;
	float Level {
		get { return level; }
		set {
			level = value;
			MyTransform.SetLocalPositionY (level);
		}
	}

	float levelPercent = 0;
	public float LevelPercent {
		get { return levelPercent; }
		set {
			levelPercent = value;
			Level = Mathf.Lerp (minLevel, maxLevel, value);
		}
	}

	public void BeginRising () {
		LevelPercent = 0;
		StartCoroutine (CoRise ());
	}

	IEnumerator CoRise () {
		
		float time = timescale;
		float eTime = 0f;
	
		while (eTime < time) {
			eTime += Time.deltaTime * rate;
			LevelPercent = Mathf.Lerp (0f, 1f, eTime / time);
			yield return null;
		}

		if (endRising != null) endRising ();
	}
}
