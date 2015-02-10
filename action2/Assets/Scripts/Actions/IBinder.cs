using UnityEngine;
using System.Collections;

namespace GameActions {
	
	public interface IBinder {
		
		IActionAcceptor BoundAcceptor { get; }
		void OnEndActions ();
	}
}