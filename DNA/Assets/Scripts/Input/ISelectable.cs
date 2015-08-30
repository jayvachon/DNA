using UnityEngine;
using System.Collections;

namespace GameInput {
	
	public interface ISelectable {
		void OnSelect ();
		void OnUnselect ();
	}
}