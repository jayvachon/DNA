using UnityEngine;
using System.Collections;
using DNA.Paths;

namespace DNA.EventSystem {

	public class MouseExitConnectionEvent : ClickConnectionEvent {

		public MouseExitConnectionEvent (ConnectionContainer connection) : base (connection) {}	
	}
}