using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Notification : MBRefs {

	public Text text;

	RectTransform rectTransform = null;
	RectTransform RectTransform {
		get {
			if (rectTransform == null) {
				rectTransform = (RectTransform)MyTransform;
			}
			return rectTransform;
		}
	}

	float yPosition = -1;
	float YPosition {
		get {
			if (yPosition == -1) {
				yPosition = RectTransform.anchoredPosition.y;
			}
			return yPosition;
		}
	}

	float outPosition = 400f;
	float inPosition = -20f;

	public void SetContent (string content) {
		text.text = content;
		StartCoroutine (CoSlide (outPosition, inPosition));
	}

	public void Close () {
		StartCoroutine (CoSlide (inPosition, outPosition, () => NotificationCenter.Instance.RemoveNotification (text.text)));
	}

	IEnumerator CoSlide (float from, float to, System.Action onEnd=null) {
		
		float time = 0.5f;
		float eTime = 0f;

		while (eTime < time) {
			eTime += Time.deltaTime;
			float progress = Mathf.SmoothStep (0, 1, eTime / time);
			RectTransform.anchoredPosition = new Vector3 (Mathf.Lerp (from, to, progress), YPosition, 0);
			yield return null;
		}

		if (onEnd != null) onEnd ();
	}
}
