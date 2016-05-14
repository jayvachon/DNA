using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace DNA.Units {

	public class SharkManager : MonoBehaviour {

		GivingTreeUnit givingTree;
		GivingTreeUnit GivingTree {
			get {
				if (givingTree == null) {
					givingTree = GameObject.FindObjectOfType (typeof (GivingTreeUnit)) as GivingTreeUnit;
				}
				return givingTree;
			}
		}

		float startRadius = 20f;

		void OnEnable () {
			// LoanManager.onUpdateLoans += OnUpdateLoans;
			// LoanManager.onUpdatePayments += OnUpdatePayments;
			Co2.Repeat (10f, () => {
				foreach (Loan loan in LoanManager.GetLoansInRepayment ())
					CreateShark (loan);
			});
		}

		/*void OnUpdatePayments (Dictionary<string, int> payments) {
			foreach (var payment in payments)
				CreateShark (payment.Key, payment.Value);
		}*/

		[DebuggableMethod ()]
		void CreateShark (Loan loan) {
			float angle = Random.Range (0, Mathf.PI * 2);
			Shark.Create (
				new Vector3 (
					startRadius * Mathf.Sin (angle), 
					GivingTree.Position.y, 
					startRadius * Mathf.Cos (angle)
				), 
				GivingTree,
				loan
			);
		}
	}
}