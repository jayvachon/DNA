using UnityEngine;
using System.Collections;
using GameInventory;

namespace GameActions {

	// TODO: Deprecate
	public class DeliverElder : DeliverItem<ElderHolder> {

		public override EnabledState EnabledState {
			get { return new DefaultEnabledState (); }
		}

		public DeliverElder () : base (0, false, false) {}

		public override void OnEnd () {
			AcceptorInventory.AddItem<ElderHolder> ();
		}
	}
}