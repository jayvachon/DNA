using UnityEngine;
using System.Collections;
using GameInventory;

namespace GameActions {

	public class HealElder : PerformerAction {

		ElderHolder holder = null;
		ElderHolder Holder {
			get {
				if (holder == null) {
					holder = Inventory.Get<ElderHolder> () as ElderHolder;
				}
				return holder;
			}
		}

		bool IsSick (Item item) {
			ElderItem elder = item as ElderItem;
			return elder.Health < 0.5f;
		}

		public HealElder (float duration) : base (duration, true, true, null) {}

		public override void OnEnd () {
			if (!Holder.Empty) {
				ElderItem sickElder = Holder.Get (IsSick) as ElderItem;
				if (sickElder != null) {
					sickElder.Health = 1f;
				}
			}
		}
	}
}