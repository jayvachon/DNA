using UnityEngine;
using System.Collections;
using GameInventory;

namespace GameActions {
	
	public class CollectElder : CollectItem<ElderHolder> {

		public CollectElder (float duration) : base (duration) {}

		public override void OnEnd () {
			
		}
	}
}