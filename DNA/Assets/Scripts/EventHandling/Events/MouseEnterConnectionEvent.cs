using UnityEngine;
using System.Collections;
using DNA.Paths;

namespace DNA.EventSystem {

	public class MouseEnterConnectionEvent : ClickConnectionEvent {

		public MouseEnterConnectionEvent (ConnectionContainer connection) : base (connection) {}	
	}
}