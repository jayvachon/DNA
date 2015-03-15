using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public delegate void ButtonPress (string id);

public class ButtonContainer : MBRefs, IPoolable {

	public Text text;
	string id = "";
	ButtonPress buttonPress;

	public void Init (string id, string inputName, ButtonPress buttonPress) {
		this.id = id;
		text.text = inputName;
		this.buttonPress = buttonPress;
	}

	public void OnPress () {
		buttonPress (id);
	}

	public void OnCreate () {}
	public void OnDestroy () {}
}
