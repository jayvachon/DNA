using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace DNA.Tasks {

	public abstract class AcceptorTask : Task {

		public delegate void OnChangeBoundCount (int count);

		public abstract System.Type AcceptedTask { get; }
		public ITaskAcceptor Acceptor { get; set; }

		BindingHandler binder;
		public BindingHandler Binder {
			get {
				if (binder == null) {
					binder = new BindingHandler (this);
				}
				return binder;
			}
		}

		public class BindingHandler {

			List<PerformerTask> active = new List<PerformerTask> ();
			Queue<PerformerTask> queued = new Queue<PerformerTask> ();
			// int bindLimit = 1;
			// AcceptorTask task;

			public BindingHandler (AcceptorTask task) {
				// this.task = task;
				// TODO: would probably be better to have this set initially				
				// bindLimit = DataManager.GetPerformerPairSettings (task.GetType ()).BindCapacity;
			}

			public void Add (PerformerTask task, int bindLimit) {

				// Ignore if no BindLimit has been set	
				if (bindLimit == 0) {
					task.OnBind ();
					return;
				}

				if (active.Count < bindLimit) {
					Bind (task);
				} else {
					queued.Enqueue (task);
					task.OnQueue ();
				}
			}

			public void Remove (PerformerTask task, int bindLimit) {

				// Ignore if no BindLimit has been set
				if (bindLimit == 0) {
					return;
				}
				
				try {
					active.Remove (task);
				} catch {
					throw new System.Exception ("The PerformerTask '" + task + "' has not been bound to the AcceptorTask '" + task + "'");
				}

				if (queued.Count > 0) {
					PerformerTask newTask = queued.Dequeue ();
					Add (newTask, bindLimit);
				}
			}

			void Bind (PerformerTask task) {
				active.Add (task);
				task.OnBind ();
			}
		}
	}
}