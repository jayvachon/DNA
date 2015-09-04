using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace DNA.Tasks {

	public static class TaskMatcher {

		public static List<PerformerTask> GetActive (ITaskPerformer performer, ITaskAcceptor acceptor) {
			return GetMatching (performer.PerformableTasks.ActiveTasks, acceptor.AcceptableTasks.ActiveTasks);
		}

		public static List<PerformerTask> GetEnabled (ITaskPerformer performer, ITaskAcceptor acceptor) {
			return GetMatching (performer.PerformableTasks.EnabledTasks, acceptor.AcceptableTasks.EnabledTasks);
		}

		public static bool HasPair (PerformerTask task, ITaskAcceptor acceptor) {
			return acceptor.AcceptableTasks.EnabledTasks.Any (x => x.GetType () == task.Settings.Pair);
		}		

		static List<PerformerTask> GetMatching (Dictionary<System.Type, PerformerTask> performerTasks, Dictionary<System.Type, AcceptorTask> acceptorTasks) {
			List<PerformerTask> matching = new List<PerformerTask> ();
			foreach (var acceptorTask in acceptorTasks) {
				PerformerTask p;
				if (performerTasks.TryGetValue (acceptorTask.Value.AcceptedTask, out p)) {
					matching.Add (p);
				}
			}
			return matching;
		}
	}
}