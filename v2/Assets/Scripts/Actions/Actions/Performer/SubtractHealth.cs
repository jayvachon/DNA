using UnityEngine;
using System.Collections;
using GameInventory;

namespace GameActions {

	public class SubtractHealth : PerformerAction {

		ElderItem elder = null;
		ElderItem Elder {
			get {
				if (elder == null) {
					elder = Performer as ElderItem;
				}
				return elder;
			}
		}

		public SubtractHealth (float duration) : base (duration, true, true, null) {}

		public override void OnEnd () {
			Elder.SubtractHealth (0.1f);
		}
	}
}