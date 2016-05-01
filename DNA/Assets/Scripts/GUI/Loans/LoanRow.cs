using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using InventorySystem;

namespace DNA {

	public class LoanRow : UIElement {

		public void OnEnable () {
			// This is stupid but necessary because the scale comes in all weird			
			RectTransform.localScale = Vector3.zero;
			Co2.WaitForFixedUpdate (() => {
				RectTransform.localScale = Vector3.one;
			});
		}

		Loan loan;

		public void Init (string resourceName, Loan loan) {
			this.loan = loan;
			GetChildComponent<Text> (0).text = resourceName;
			loan.onUpdate += OnUpdateLoan;
			OnUpdateLoan ();
		}

		void OnUpdateLoan () {
			if (loan.Owed == 0 || loan.Status == Loan.LoanStatus.Defaulted) {
				ObjectPool.Destroy<LoanRow> (this);
			}
			GetChildComponent<Text> (1).text = loan.StatusDetails;
			GetChildComponent<Text> (2).text = loan.Owed.ToString ();
			GetChildComponent<Text> (3).text = loan.Payment.ToString ();
		}
	}
}