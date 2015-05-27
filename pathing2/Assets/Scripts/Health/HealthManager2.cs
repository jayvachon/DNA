using UnityEngine;
using System.Collections;

public delegate void OnChangeDegradeRate ();

public class HealthManager2 {

	public OnChangeDegradeRate onChangeDegradeRate;

	float minRate = 0.1f;
	float maxRate = 10f;

	float degradeRate = 1f;
	public float DegradeRate {
		get { return degradeRate * TimerValues.Instance.Year; }
	}

	public void SetDegradeRate (float quality) {
		degradeRate = Mathf.Lerp (minRate, maxRate, quality);
		if (onChangeDegradeRate != null) onChangeDegradeRate ();
	}
}
