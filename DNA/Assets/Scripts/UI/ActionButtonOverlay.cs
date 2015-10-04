using UnityEngine;
using System.Collections;
using UnityEngine.UI;

// Deprecate

//public delegate void ActionButtonPress (string id);

public class ActionButtonOverlay : MonoBehaviour {

	public Text text;
	string id = "";
	ActionButtonPress actionButtonPress;

	public void Init (string id, string inputName, ActionButtonPress actionButtonPress) {
		this.id = id;
		text.text = inputName;
		this.actionButtonPress = actionButtonPress;
	}

	public void OnPress () {
		actionButtonPress (id);
	}
}
