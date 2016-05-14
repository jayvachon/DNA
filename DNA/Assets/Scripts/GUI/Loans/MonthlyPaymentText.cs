using UnityEngine;
using System.Collections;

namespace DNA {

	public class MonthlyPaymentText : UIElement {

		public string loanGroup;

		void OnEnable () {
			// LoanManager.onUpdateLoans += OnUpdateLoanGroups;
		}

		/*void OnUpdateLoanGroups () {
			foreach (Loan l in LoanManager.Inventory[loanGroup].Items) {
				l.onUpdate += OnUpdateLoans;
			}
		}*/

		/*void OnUpdateLoans () {
			int payment = 0;
			foreach (Loan l in LoanManager.Inventory[loanGroup].Items) {
				if (l.Owed == 0) {
					l.onUpdate -= OnUpdateLoans;
					continue;
				}
				if (l.Status != Loan.LoanStatus.Grace) {
					payment += l.Payment;
				}
			}
			Text.text = loanGroup + ": " + payment;
		}*/
	}
}