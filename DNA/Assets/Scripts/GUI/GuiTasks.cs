using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DNA.InputSystem;
using DNA.Tasks;

public class GuiTasks : GuiSelectableListener {

	void Awake () {
		Init ();
	}

	protected override void OnUpdateSelection (List<ISelectable> selected) {

		if (selected.Count == 0)
			return;

		List<ITaskPerformer> performers = selected
			.FindAll (x => x is ITaskPerformer)
			.ConvertAll (x => x as ITaskPerformer);
		List<PerformerTask> tasks = TaskMatcher.GetTasksInCommon (performers);
		foreach (PerformerTask t in tasks) {
			// TODO: display these in the gui
		}
	}
}
