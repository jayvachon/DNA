using UnityEngine;
using System.Collections;

namespace DNA.Tasks {

	public abstract class AcceptorTask : Task {
		public abstract System.Type AcceptedTask { get; }
		public ITaskAcceptor Acceptor { get; set; }
	}
}