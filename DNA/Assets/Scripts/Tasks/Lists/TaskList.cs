using UnityEngine;
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

	public delegate void OnTasksUpdate ();

	public abstract class TaskList<T> where T : Task {

		public OnTasksUpdate onTasksUpdate;

		// All tasks within this TaskList
		Dictionary<System.Type, Task> tasks = new Dictionary<System.Type, Task> ();

		// Inactive tasks are not considered for enabling
		Dictionary<System.Type, T> activeTasks = new Dictionary<System.Type, T> ();
		public Dictionary<System.Type, T> ActiveTasks {
			get { return activeTasks; }
			protected set { activeTasks = value; }
		}

		// Tasks are enabled if their EnabledState is true
		Dictionary<System.Type, T> enabledTasks = new Dictionary<System.Type, T> ();
		public Dictionary<System.Type, T> EnabledTasks {
			get { return enabledTasks; }
			protected set { enabledTasks = value; }
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
			tasks.Add (taskType, task);
			ActiveTasks.Add (taskType, task as T);
			EnabledTasks.Add (taskType, task as T);
			NotifyTasksUpdated ();
		}

		public void SetActive (System.Type taskType, bool active) {
			if (active) 
				Activate (taskType);
			else
				Deactivate (taskType);
			NotifyTasksUpdated ();
			OnSetActive ();	
		}

		void Activate (System.Type taskType) {
			Task task;
			if (tasks.TryGetValue (taskType, out task)) {
				if (!ActiveTasks.ContainsKey (taskType)) {
					ActiveTasks.Add (taskType, (T)task);
				}
			}
			if (task.Enabled) EnabledTasks.Add (taskType, (T)task);
		}

		void Deactivate (System.Type taskType) {
			EnabledTasks.Remove (taskType);
			ActiveTasks.Remove (taskType);
		}

		public void ActivateAll () {
			ActiveTasks.Clear ();
			foreach (var task in tasks) {
				ActiveTasks.Add (task.Key, task.Value as T);
			}
			NotifyTasksUpdated ();
			OnSetActive ();
		}

		public void DeactivateAll () {
			ActiveTasks.Clear ();
			NotifyTasksUpdated ();
			OnSetActive ();
		}

		public U Get<U> () where U : Task {
			try {
				return tasks[typeof (U)] as U;
			} catch {
				throw new System.Exception ("The task '" + typeof (U) + "' does not exist in the list\n");
			}
		}

		public T GetEnabledTask () {
			foreach (var task in enabledTasks) {
				return task.Value;
			}
			return null;
		}

		public T GetActiveTask () {
			foreach (var task in activeTasks) {
				return task.Value;
			}
			return null;
		}

		public bool TaskActive (System.Type taskType) {
			return ActiveTasks.ContainsKey (taskType);
		}

		public bool TaskEnabled (System.Type taskType) {
			return EnabledTasks.ContainsKey (taskType);
		}

		public virtual void RefreshEnabledTasks () {
			EnabledTasks.Clear ();
			foreach (var keyval in ActiveTasks) {
				Task task = keyval.Value;
				if (task.Enabled)
					EnabledTasks.Add (keyval.Key, task as T);
			}
			NotifyTasksUpdated ();
		}

		/*public List<string> GetBoundTasks (List<string> acceptorTasks) {
			return acceptorTasks.FindAll (x => Has (x));
		}

		public bool AcceptorHasPair (ITaskAcceptor acceptor, Task unpairedTask) {
			return unpairedTask.EnabledState.AttemptPair (acceptor);
		}

		public List<string> GetPairedTasksBetweenAcceptors (ITaskAcceptor a, ITaskAcceptor b) {
			AcceptableTasks aa = a.AcceptableTasks;
			AcceptableTasks ba = b.AcceptableTasks;
			aa.RefreshEnabledTasks ();
			ba.RefreshEnabledTasks ();
			List<string> paired = new List<string> ();
			Dictionary<string, AcceptorTask> aTasks = aa.ActiveTasks;
			foreach (var task in aTasks) {
				if (task.Value.EnabledState.AttemptPair (b)) 
					paired.Add (task.Key);
			}
			return paired;
		}

		public bool HasMatchingTask (Task task) {
			return activeTasks.ContainsKey (task.Name);
		}*/

		public void NotifyTasksUpdated () {
			if (onTasksUpdate != null) onTasksUpdate ();
		}

		public virtual void OnSetActive () {}

		/**
		 *	Debugging
		 */

		/*public List<PerformerTask> EnabledTasksList {
			get {
				List<PerformerTask> list = new List<PerformerTask> ();
				foreach (var task in EnabledTasks) {
					list.Add (task.Value as PerformerTask);
				}
				return list;
			}
		}

		public virtual void Print () {
			foreach (var task in tasks) {
				Debug.Log (task.Key 
					+ " enabled = " + task.Value.Enabled 
					+ ", active = " + task.Value.Active);
			}
		}

		public void PrintEnabled () {
			foreach (var task in EnabledTasks) {
				Debug.Log (task.Key);
			}
		}*/
	}
}