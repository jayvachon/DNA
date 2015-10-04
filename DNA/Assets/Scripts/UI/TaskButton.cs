using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using DNA.Tasks;

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

	PerformerTask task;

	public void Init (PerformerTask task) {
		this.task = task;
		Text.text = task.Settings.Title;
	}

	public void OnPress () {
		task.Start ();
	}
}
