using UnityEngine;
using System.Collections;
using DNA.Tasks;
using InventorySystem;

namespace DNA {

	public class BorrowMilkshakesButton : BorrowLoanButton {

		protected override InventoryTask BorrowTask {
			get { 
				if (!PerformableTasks.Has (typeof (BorrowLoan<MilkshakeLoanGroup>))) {
					PerformableTasks.Add (new BorrowLoan<MilkshakeLoanGroup> ());
				}
				return PerformableTasks[typeof (BorrowLoan<MilkshakeLoanGroup>)] as InventoryTask; 
			}
		}

		void OnEnable () {
			SetText ("milkshakes", DataManager.GetLoanSettings (typeof (Loan<MilkshakeGroup>)).Amount);
		}
	}
}