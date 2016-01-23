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
		}
	}
}