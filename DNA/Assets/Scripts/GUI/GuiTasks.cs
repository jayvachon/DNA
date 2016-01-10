using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using DNA.InputSystem;
using DNA.Tasks;
using DNA;

public class GuiTasks : GuiSelectableListener {

	public List<TaskButton> buttons;
	List<ISelectable> selected;

	void Awake () {
		Init ();
	}

	public void SetButtons (List<PerformerTask> tasks) {
		bool hasTask = false;
		DisableButtons ();
		foreach (PerformerTask t in tasks) {
			if (t.Settings.Title != null && t.Settings.Title != "") {
				EnableButton (t);
				hasTask = true;
			}
		}
		if (hasTask) {
			foreach (var g in Player.Instance.Inventory.Groups) {
				g.Value.onUpdate += UpdateActiveState;
			}
		}
		SetGroupActive (hasTask);
	}

	protected override void OnUpdateSelection (List<ISelectable> selected) {

		this.selected = selected;

		if (selected.Count == 0) {
			SetButtons (Player.Instance.PerformableTasks.ActiveTasks.Values.ToList ());
			return;
		}

		List<ITaskPerformer> performers = selected
			.FindAll (x => x is ITaskPerformer)
			.ConvertAll (x => x as ITaskPerformer);

		List<PerformerTask> tasks = TaskMatcher.GetTasksInCommon (performers, true);
		SetButtons (tasks);
	}

	void UpdateActiveState () {
		foreach (var g in Player.Instance.Inventory.Groups) {
			g.Value.onUpdate -= UpdateActiveState;
		}
		OnUpdateSelection (selected);
	}

	void EnableButton (PerformerTask t) {
		TaskButton button = buttons.Find (x => !x.gameObject.activeSelf);
		if (button != null) {
			button.gameObject.SetActive (true);
			button.Init (t);
			button.Button.interactable = t.Enabled;
		}
	}

	void DisableButtons () {
		foreach (TaskButton button in buttons)
			button.gameObject.SetActive (false);
	}
}
