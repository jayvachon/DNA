using UnityEngine;
using System.Collections;
using GameInventory;

namespace GameActions {

	public delegate void OnElderHealed ();

	public class HealElder : InventoryAction<ElderHolder> {

		OnElderHealed onElderHealed;

		bool IsSick (Item item) {
			ElderItem elder = item as ElderItem;
			return elder.HealthManager.Sick;
		}

		public HealElder (float duration, OnElderHealed onElderHealed=null) : base (duration, false, false, null) {
			this.onElderHealed += onElderHealed;
		}

		public override void OnEnd () {
			if (!Holder.Empty) {
				ElderItem sickElder = Holder.Get (IsSick) as ElderItem;
				if (sickElder != null) {
					sickElder.HealthManager.StopSickness ();
				}
				if (onElderHealed != null) onElderHealed ();
			}
		}
	}
}