using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using DNA.InputSystem;
using DNA.Tasks;
using DNA;

public class GuiTasks : GuiSelectableListener {

	public List<TaskButton> buttons;

	void Awake () {
		Init ();
		DisplableButtons ();
	}

	public void SetButtons (List<PerformerTask> tasks) {
		bool hasTask = false;
		DisplableButtons ();
		foreach (PerformerTask t in tasks) {
			if (t.Settings.Title != "") {
				EnableButton (t);
				hasTask = true;
			}
		}
		SetGroupActive (hasTask);
	}

	protected override void OnUpdateSelection (List<ISelectable> selected) {

		if (selected.Count == 0) {
			//SetGroupActive (false);
			SetButtons (Player.Instance.PerformableTasks.EnabledTasks.Values.ToList ());
			return;
		}

		List<ITaskPerformer> performers = selected
			.FindAll (x => x is ITaskPerformer)
			.ConvertAll (x => x as ITaskPerformer);

		List<PerformerTask> tasks = TaskMatcher.GetTasksInCommon (performers);
		SetButtons (tasks);
	}

	void EnableButton (PerformerTask t) {
		TaskButton button = buttons.Find (x => !x.gameObject.activeSelf);
		if (button != null) {
			button.gameObject.SetActive (true);
			button.Init (t);
		}
	}

	void DisplableButtons () {
		foreach (TaskButton button in buttons)
			button.gameObject.SetActive (false);
	}
}
