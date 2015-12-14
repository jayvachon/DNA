using UnityEngine;
using System.Collections;

public class ProgressBar : MonoBehaviour {

	public RectTransform back;
	public RectTransform mercury;

	float fillSize;

	void OnEnable () {
		mercury.sizeDelta = back.sizeDelta;
		fillSize = back.sizeDelta.x;
	}

	public void SetProgress (float p) {
		mercury.sizeDelta = new Vector2 (fillSize * p, mercury.sizeDelta.y);
	}

	void Update () {
		transform.LookAt (Camera.main.transform);
	}
}
