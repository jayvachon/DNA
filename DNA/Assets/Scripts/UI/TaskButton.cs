using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using DNA.Tasks;
using DNA;

public class TaskButton : MBRefs {

	Text text = null;
	Text Text {
		get {
			if (text == null) {
				text = MyTransform.GetChild (0).GetComponent<Text> ();
			}
			return text;
		}
	}

	Button button = null;
	public Button Button {
		get {
			if (button == null) {
				button = GetComponent<Button> ();
			}
			return button;
		}
	}

	PerformerTask task;

	public void Init (PerformerTask task) {
		this.task = task;
		Text.text = task.Settings.Title;
	}

	public void OnPress () {
		if (task is ConstructRoad || task is ConstructUnit) {
			Player.Instance.SetConstructionPen (task.GetType ());
		} else {
			task.Start ();
		}
	}
}
