using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using InventorySystem;

namespace DNA {

	public class LoanList : UIElement {

		void Awake () {
			((CoffeeLoanGroup)LoanManager.Inventory["Coffee"]).onAdd += OnAddCoffeeLoan;
			((MilkshakeLoanGroup)LoanManager.Inventory["Milkshakes"]).onAdd += OnAddMilkshakeLoan;
		}

		void OnAddCoffeeLoan (List<Loan<CoffeeGroup>> loan) {
			LoanRow row = ObjectPool.Instantiate<LoanRow> ();
			row.transform.SetParent (RectTransform);
			row.Init ("Coffee", loan[0]);
		}

		void OnAddMilkshakeLoan (List<Loan<MilkshakeGroup>> loan) {
			LoanRow row = ObjectPool.Instantiate<LoanRow> ();
			row.transform.SetParent (RectTransform);
			row.Init ("Milkshakes", loan[0]);
		}
	}
}