using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace DNA.Tasks {

	/**
	 *	TaskList
	 *	- PerformableTasks
	 *		> PerformerTask
	 *		> ITaskPerformer
	 *	- AcceptableTasks
	 *		> AcceptorTask
	 *		> ITaskAcceptor
	 */

	public abstract class TaskList<T> where T : Task {

		// All tasks within this TaskList
		Dictionary<System.Type, Task> tasks = new Dictionary<System.Type, Task> ();

		// Inactive tasks are not considered for enabling
		Dictionary<System.Type, T> activeTasks = new Dictionary<System.Type, T> ();
		public Dictionary<System.Type, T> ActiveTasks {
			get { return activeTasks; }
			protected set { activeTasks = value; }
		}

		// Tasks are enabled if their EnabledState is true
		public Dictionary<System.Type, T> EnabledTasks {
			get { 
				// TODO: linq this
				Dictionary<System.Type, T> enabledTasks = new Dictionary<System.Type, T> ();
				foreach (var keyval in ActiveTasks) {
					Task task = keyval.Value;
					if (task.Enabled)
						enabledTasks.Add (keyval.Key, task as T);
				}
				return enabledTasks;
			}
		}

		public T this[System.Type taskType] {
			get { 
				try {
					return (T)tasks[taskType]; 
				} catch {
					throw new System.Exception ("Could not find the task '" + taskType + "'");
				}
			}
		}

		public void AddTask (Task task) {
			System.Type taskType = task.GetType ();
			if (tasks.ContainsKey (taskType))
				throw new System.Exception ("The task list '" + this + "' already contains the task '" + task + "'");
			tasks.Add (taskType, task);
			ActiveTasks.Add (taskType, task as T);
		}

		public void SetActive (System.Type taskType, bool active) {
			if (active) 
				Activate (taskType);
			else
				Deactivate (taskType);
		}

		void Activate (System.Type taskType) {
			if (!ActiveTasks.ContainsKey (taskType)) {
				ActiveTasks.Add (taskType, (T)tasks[taskType]);
			}
		}

		void Deactivate (System.Type taskType) {
			ActiveTasks.Remove (taskType);
		}

		public void ActivateAll () {
			ActiveTasks.Clear ();
			foreach (var task in tasks) {
				ActiveTasks.Add (task.Key, task.Value as T);
			}
		}

		public void DeactivateAll () {
			ActiveTasks.Clear ();
		}

		public bool Has (System.Type taskType) {
			return tasks.ContainsKey (taskType);
		}

		/**
		 *	Debugging
		 */

		public virtual void Print () {
			Debug.Log ("========== ALL TASKS ==========");
			foreach (var task in tasks)
				Debug.Log (task.Key);

			Debug.Log ("========== ACTIVE TASKS ==========");
			foreach (var task in ActiveTasks)
				Debug.Log (task.Key);

			Debug.Log ("========== ENABLED TASKS ==========");
			foreach (var task in EnabledTasks)
				Debug.Log (task.Key);				
		}
	}
}