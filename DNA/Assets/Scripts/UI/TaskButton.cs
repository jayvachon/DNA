using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using DNA.Tasks;
using DNA;

public class TaskButton : UIElement {

	/*Text text = null;
	Text Text {
		get {
			if (text == null) {
				// text = MyTransform.GetChild (0).GetComponent<Text> ();
				text = MyTransform.GetChild<Text> ();
			}
			return text;
		}
	}*/

	/*Button button = null;
	public Button Button {
		get {
			if (button == null) {
				button = GetComponent<Button> ();
			}
			return button;
		}
	}*/

	PerformerTask task;
	bool hasText; // BAD hack

	public void Init (PerformerTask task, bool hasText=true) {

		this.task = task;
		this.hasText = hasText;

		if (hasText) {
			ButtonText.text = task.Settings.Title;
			if (task is CostTask) {
				CostTask costTask = task as CostTask;
				ButtonText.text += " (";
				int count = costTask.Costs.Count;
				foreach (var c in costTask.Costs) {
					ButtonText.text += c.Value;
					if (c.Key == "Coffee")
						ButtonText.text += "C";
					else
						ButtonText.text += "M";
					count --;
					if (count > 0)
						ButtonText.text += ", ";
				}
				ButtonText.text += ")";
			}
		}

		RemoveButtonListeners ();
		AddButtonListener (OnPress);
	}

	public void OnPress () {
		if (task is ConstructRoad || task is ConstructUnit) {
			TaskPen.Set (task);
		} else {
			task.Start ();
		}
		if (hasText) // BAD hack - this is to get the road button working
			Init (task);
	}
}
