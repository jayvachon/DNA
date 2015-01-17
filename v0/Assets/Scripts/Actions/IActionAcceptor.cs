using UnityEngine;
using System.Collections;

namespace GameActions {
	
	// Tells an IActionable which Actions it can run
	public interface IActionAcceptor {

		AcceptedActions AcceptedActions { get; set; }
	}
}