using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using InventorySystem;

namespace DNA {

	public static class LoanManager {

		static float repaymentTime = 60f;

		static Inventory inventory;
		public static Inventory Inventory {
			get {
				if (inventory == null) {
					inventory = new Inventory ();
					inventory.Add (new MilkshakeLoanGroup (5));
					inventory.Add (new CoffeeLoanGroup (5));
					Coroutine.WaitForSeconds (repaymentTime, ElapseTime);
				}
				return inventory;
			}
		}

		static void ElapseTime () {

			foreach (var group in Inventory.Groups) {
				foreach (Item item in group.Value.Items) {
					((Loan)item).AddTime ();
				}
			}

			Coroutine.WaitForSeconds (repaymentTime, ElapseTime);
		}
	}
}