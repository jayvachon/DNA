using UnityEngine;
using System.Collections;
using GameInventory;

namespace GameActions {

	public abstract class AcceptorAction : Action {

		public IActionAcceptor Acceptor { get; set; }
		
		Inventory inventory = null;
		protected Inventory Inventory {
			get {
				if (inventory == null && Acceptor is IInventoryHolder) {
					IInventoryHolder holder = Acceptor as IInventoryHolder;
					inventory = holder.Inventory;
				}
				if (inventory == null) {
					Debug.LogError ("ActionAcceptor does not implement IInventoryHolder");
				}
				return inventory;
			}
		}

		AcceptCondition acceptCondition = null;

		public AcceptCondition AcceptCondition {
			get {
				if (acceptCondition == null) {
					acceptCondition = new DefaultCondition ();
				}
				if (acceptCondition.Inventory == null) {
					acceptCondition.Inventory = Inventory;
				}
				return acceptCondition; 
			}
		}

		public bool ConditionMet {
			get { 
				if (acceptCondition == null) {
					return true;
				}
				return acceptCondition.Acceptable; 
			}
		}

		public AcceptorAction (AcceptCondition acceptCondition) {
			this.acceptCondition = acceptCondition;
		}

		public void Bind (Inventory inventory) {
			if (AcceptCondition != null) {
				AcceptCondition.PerformerInventory = inventory;
			}
		}
	}
}