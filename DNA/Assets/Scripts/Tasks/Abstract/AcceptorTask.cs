using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace DNA.Tasks {

	public abstract class AcceptorTask : Task {

		public delegate void OnChangeBoundCount (int count);

		public abstract System.Type AcceptedTask { get; }
		public ITaskAcceptor Acceptor { get; set; }
		public OnChangeBoundCount onChangeBoundCount;

		List<PerformerTask> boundTasks = new List<PerformerTask> ();

		public int BoundCount {
			get { return boundTasks.Count; }
		}

		public void Bind (PerformerTask task) {
			boundTasks.Add (task);
			SendChangeBoundCountMessage ();
		}

		public void Unbind (PerformerTask task) {
			if (boundTasks.Contains (task)) {
				boundTasks.Remove (task);
				SendChangeBoundCountMessage ();
			}
			/*try {
				boundTasks.Remove (task);
				SendChangeBoundCountMessage ();
			} catch {
				throw new System.Exception (task + " has not been bound to " + this + " and cannot be unbound");
			}*/
		}

		void SendChangeBoundCountMessage () {
			if (onChangeBoundCount != null)
				onChangeBoundCount (BoundCount);
		}
	}
}