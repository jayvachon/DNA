using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using InventorySystem;

namespace DNA {

	public static class LoanManager {

		static Inventory inventory;
		public static Inventory Inventory {
			get {
				if (inventory == null) {
					inventory = new Inventory ();
					inventory.Add (new MilkshakeLoanGroup (4));
					inventory.Add (new CoffeeLoanGroup (4));
				}
				return inventory;
			}
		}

		public static List<Loan> GetLoans () {

			List<Loan> loans = new List<Loan> ();

			foreach (var group in Inventory.Groups) {
				foreach (Item item in group.Value.Items)
					loans.Add ((Loan)item);
			}

			return loans;
		}
	}
}