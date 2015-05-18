using UnityEngine;
using System.Collections;
using GameInventory;

namespace GameActions {

	public class DeliverUnpairedItem<T> : DeliverItem<T> where T : ItemHolder {

		public override EnabledState EnabledState {
			get { return new DefaultEnabledState (); }
		}

		public DeliverUnpairedItem () : base (0, false, false) {}

		public override void OnEnd () {
			AcceptorInventory.AddItem<T> ();
		}
	}
}