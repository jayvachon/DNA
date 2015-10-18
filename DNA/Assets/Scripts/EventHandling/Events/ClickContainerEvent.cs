using UnityEngine;
using System.Collections;
using DNA.Paths;

namespace DNA.EventSystem {

	public class ClickConnectionEvent : GameEvent {

		public readonly ConnectionContainer Container;
		public readonly Connection Connection;

		public ClickConnectionEvent (ConnectionContainer container) {
			Container = container;
			Connection = Container.Connection;
		}
	}
}