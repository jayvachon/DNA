using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;

public class ConstructPrompt : MonoBehaviour {

	public GameObject container;
	public Text confirmText;
	public Button confirmButton;
	public Button cancelButton;

	void Awake () {
		Close ();
	}

	public void Open (string text, UnityAction onConfirm, UnityAction onDeny=null) {
		confirmText.text = text;
		confirmButton.onClick.RemoveAllListeners ();
		confirmButton.onClick.AddListener (onConfirm);
		confirmButton.onClick.AddListener (Close);
		cancelButton.onClick.AddListener (Close);
		if (onDeny != null)
			cancelButton.onClick.AddListener (onDeny);
		container.gameObject.SetActive (true);
	}

	public void Close () {
		container.gameObject.SetActive (false);
	}
}
