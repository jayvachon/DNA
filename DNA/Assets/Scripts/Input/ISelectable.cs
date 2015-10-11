using UnityEngine;
using System.Collections;

namespace DNA.InputSystem {
	
	public interface ISelectable {
		SelectSettings SelectSettings { get; }
		void OnSelect ();
		void OnUnselect ();
	}
}