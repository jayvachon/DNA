using UnityEngine;
using UnityEngine.UI;
using System.Collections;

// A base class for all UI elements to inherit from.
// Allows for quick access to commonly used components and callbacks.

public class UIElement : MBRefs {

	RectTransform rectTransform = null;
	protected RectTransform RectTransform {
		get {
			if (rectTransform == null) {
				rectTransform = GetComponent<RectTransform> ();
			}
			return rectTransform;
		}
	}

	Image image = null;
	protected Image Image {
		get {
			if (image == null) {
				image = GetComponent<Image> ();
			}
			return image;
		}
	}

	Button button = null;
	protected Button Button {
		get {
			if (button == null) {
				button = GetComponent<Button> ();
				button.onClick.AddListener (OnButtonPress);
			}
			return button;
		}
	}

	Text buttonText = null;
	protected Text ButtonText {
		get {
			if (buttonText == null) {
				buttonText = Button.transform.GetChild(0).GetComponent<Text> ();
			}
			return buttonText;
		}
	}

	Text text = null;
	protected Text Text {
		get {
			if (text == null) {
				text = GetComponent<Text> ();
			}
			return text;
		}
	}

	protected T GetChildComponent<T> (int childIndex) where T : MonoBehaviour {
		return RectTransform.GetChild (childIndex).GetComponent<T> () as T;
	}

	protected virtual void OnButtonPress () {}
}
