using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;

public class ConstructPrompt : MonoBehaviour {

	public GameObject container;
	public Text confirmText;
	public Button confirmButton;

	void Awake () {
		Close ();
	}

	public void Open (string text, UnityAction onPress) {
		confirmText.text = text;
		confirmButton.onClick.RemoveAllListeners ();
		confirmButton.onClick.AddListener (onPress);
		container.gameObject.SetActive (true);
	}

	public void Close () {
		container.gameObject.SetActive (false);
	}
}
