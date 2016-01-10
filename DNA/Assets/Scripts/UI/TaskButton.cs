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
		if (task is CostTask) {
			CostTask costTask = task as CostTask;
			Text.text += " (";
			int count = costTask.Costs.Count;
			foreach (var c in costTask.Costs) {
				Text.text += c.Value;
				if (c.Key == "Coffee")
					Text.text += "C";
				else
					Text.text += "M";
				count --;
				if (count > 0)
					Text.text += ", ";
			}
			Text.text += ")";
		}
	}

	public void OnPress () {
		if (task is ConstructRoad || task is ConstructUnit) {
			TaskPen.Set (task);
		} else {
			task.Start ();
		}
		Init (task);
	}
}
