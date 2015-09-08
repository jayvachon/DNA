using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace DNA.Tasks {

	public class PerformableTasks : TaskList<PerformerTask> {

		ITaskPerformer performer;

		public PerformableTasks (ITaskPerformer performer) {
			this.performer = performer;
		}

		public PerformerTask Add (PerformerTask task) {
			task.Performer = performer;
			AddTask (task);
			return task;
		}
	}
}