using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace DNA.Units {

	public class SharkManager : MonoBehaviour {

		float startRadius = 10f;

		void OnEnable () {
			Co2.Repeat (10f, () => {
				foreach (Loan loan in LoanManager.GetLoans ())
					CreateShark (loan);
			});
		}

		void CreateShark (Loan loan) {
			float angle = Random.Range (0, Mathf.PI * 2);
			Shark.Create (
				new Vector3 (
					startRadius * Mathf.Sin (angle), 
					GivingTreeUnit.Instance.Position.y, 
					startRadius * Mathf.Cos (angle)
				), 
				GivingTreeUnit.Instance,
				loan
			);
		}
	}
}