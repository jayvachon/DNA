using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ProgressBar : MBRefs {

	public RectTransform back;
	public RectTransform mercury;

	Image MercuryImage {
		get { return back.GetComponent<Image> (); }
	}

	float fillSize;

	void OnEnable () {
		mercury.sizeDelta = back.sizeDelta;
		fillSize = back.sizeDelta.x;
		SetColor (Color.red);
	}

	public void SetColor (Color color) {
		MercuryImage.color = color;
	}

	public void SetProgress (float p) {
		mercury.sizeDelta = new Vector2 (fillSize * p, mercury.sizeDelta.y);
	}

	void Update () {
		MyTransform.LookAt (Camera.main.transform);
	}
}
