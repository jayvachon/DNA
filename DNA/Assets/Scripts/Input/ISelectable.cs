using UnityEngine;
using System.Collections;

namespace DNA.InputSystem {
	
	public interface ISelectable {
		void OnSelect ();
		void OnUnselect ();
	}
}