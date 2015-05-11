using UnityEngine;
using System.Collections;
using GameInventory;

namespace GameActions {

	public delegate void OnElderHealed ();

	public class HealElder : InventoryAction<ElderHolder> {

		public override string Name {
			get { return "HealElder"; }
		}

		OnElderHealed onElderHealed;

		bool IsSick (Item item) {
			ElderItem elder = item as ElderItem;
			return elder.HealthManager.Sick;
		}

		public HealElder (OnElderHealed onElderHealed=null) : base (-1, false, false) {
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