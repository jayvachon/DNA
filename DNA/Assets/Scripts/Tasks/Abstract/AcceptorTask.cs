using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace DNA.Tasks {

	public abstract class AcceptorTask : Task {

		public abstract System.Type AcceptedTask { get; }
		public ITaskAcceptor Acceptor { get; set; }

		List<PerformerTask> boundTasks = new List<PerformerTask> ();

		public int BoundCount {
			get { return boundTasks.Count; }
		}

		public void Bind (PerformerTask task) {
			boundTasks.Add (task);
		}

		public void Unbind (PerformerTask task) {
			try {
				boundTasks.Remove (task);
			} catch {
				throw new System.Exception (task + " has not been bound to " + this + " and cannot be unbound");
			}
		}
	}
}