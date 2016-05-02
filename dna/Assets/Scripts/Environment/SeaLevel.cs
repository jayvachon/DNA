using UnityEngine;
using System.Collections;

public class SeaLevel {

	public float Level {
		get { return Mathf.Lerp (Min, Max, val); }
		set { val = Mathf.InverseLerp (Min, Max, val); }
	}

	public float Fill {
		get { return val; }
		set { val = Mathf.Clamp01 (value); }
	}

	public readonly float Min, Max;
	float val;

	public SeaLevel (float min, float max) {
		this.Min = min;
		this.Max = max;
		this.val = 0;
	}

	public void Swell (float duration, bool repeat=false) {
		Co2.StartCoroutine (duration, (float p) => {
			Fill = Mathf.Sin (Mathf.PI * p);
		}, () => {
			if (repeat)
				Swell (duration, true);
		});
	}
}
