using UnityEngine;
using System.Collections;

namespace DNA.Tasks {

	public class AcceptableTasks : TaskList<AcceptorTask> {

		ITaskAcceptor acceptor;

		public AcceptableTasks (ITaskAcceptor acceptor) {
			this.acceptor = acceptor;
		}

		public void Add (AcceptorTask task) {
			task.Acceptor = acceptor;
			AddTask (task);
		}
	}
}