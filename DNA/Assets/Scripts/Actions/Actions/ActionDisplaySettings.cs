using UnityEngine;
using System.Collections;

// TODO: Not currently being used, but this should replace the "Inputs" dictionary on PerformableActions

namespace GameActions {

	public class ActionDisplaySettings {

		public readonly string id;
		public string title;

		public ActionDisplaySettings (string id, string title) {
			this.id = id;
			this.title = title;
		}
	}
}