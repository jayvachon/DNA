using UnityEngine;
using System.Collections;
using InventorySystem;

namespace DNA.Tasks {

	public class BorrowLoan<T> : InventoryTask<T> where T : ItemGroup {

		public override bool Enabled {
			get { return !Group.Full && !LoanManager.Defaulted; }
		}

		protected override Inventory Inventory {
			get { return LoanManager.Inventory; }
		}

		protected override void OnEnd () {

			Loan loan = Group.Add () as Loan;

			// Loan group must have same ID as the item group it represents			
			Player.Instance.Inventory[Group.ID].Add (loan.Amount);
		}
	}
}