using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using DNA.Tasks;
using DNA;

// working around an issue with ObjectPool - should delete this script and use the real TaskButton

public class TaskButton2 : UIElement {

	PerformerTask task;
	bool hasText;

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

	void OnPress () {
		if (task is ConstructRoad || task is ConstructUnit) {
			TaskPen.Set (task);
		} else {
			task.Start ();
		}
		// Init (task, hasText);
	}
}
