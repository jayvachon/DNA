using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace GameActions {

	public class AcceptableActions : ActionList {

		public void Add (string id) {
			AddAction (id);
		}
	}
}