using UnityEngine;
using System.Collections;

namespace DNA.Tasks {

	public class PerformableTasks : TaskList<PerformerTask> {

		ITaskPerformer performer;

		public PerformableTasks (ITaskPerformer performer) {
			this.performer = performer;
		}

		public void Add (PerformerTask task) {
			task.Performer = performer;
			AddTask (task);
		}
	}
}