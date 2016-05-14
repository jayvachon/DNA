using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using DNA.Tasks;

namespace DNA {

	public abstract class BorrowLoanButton : UIElement, ITaskPerformer {

		public RectTransform loanList;

		protected abstract InventoryTask BorrowTask { get; }

		PerformableTasks performableTasks;
		public PerformableTasks PerformableTasks {
			get {
				if (performableTasks == null) {
					performableTasks = new PerformableTasks (this);
				}
				return performableTasks;
			}
		}

		void Awake () {
			// LoanManager.onUpdateLoans += OnUpdateLoans;
			// LoanManager.onUpdatePayments += OnUpdatePayments;
			// OnUpdatePayments (null);
		}

		/*void OnUpdatePayments (Dictionary<string, int> payments) {
			Button.interactable = BorrowTask.Enabled;
		}*/

		protected void SetText (string resource, int amount) {
			ButtonText.text = "Borrow " + amount.ToString () + " " + resource;
		}

		protected override void OnButtonPress () {
			BorrowTask.Start ();
		}
	}
}