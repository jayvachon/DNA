using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DNA.Tasks;
using InventorySystem;

namespace DNA {

	public class BorrowCoffeeButton : BorrowLoanButton {

		protected override InventoryTask BorrowTask {
			get { 
				if (!PerformableTasks.Has (typeof (BorrowLoan<CoffeeLoanGroup>))) {
					PerformableTasks.Add (new BorrowLoan<CoffeeLoanGroup> ());
				}
				return PerformableTasks[typeof (BorrowLoan<CoffeeLoanGroup>)] as InventoryTask; 
			}
		}

		void OnEnable () {
			SetText ("coffee", DataManager.GetLoanSettings (typeof (Loan<CoffeeGroup>)).Amount);
			((CoffeeLoanGroup)LoanManager.Inventory["Coffee"]).onAdd += OnAddLoan;
		}

		void OnAddLoan (List<Loan<CoffeeGroup>> loan) {
			LoanRow row = ObjectPool.Instantiate<LoanRow> ();
			row.transform.SetParent (loanList);
			row.Init ("Coffee", loan[0]);
		}
	}
}