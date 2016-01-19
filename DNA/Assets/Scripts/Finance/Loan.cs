using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using InventorySystem;
using DNA.Models;

namespace DNA {

	public class Loan : Item {

		public override string Name {
			get { return "Loan"; }
		}

		public int Amount {
			get { return settings.Amount; }
		}

		public int Payment {
			get {
				// https://upload.wikimedia.org/math/e/b/0/eb0d554c1ead49a1a0bd3155b764ec0c.png				
				float c = settings.InterestRate;
				float n = settings.RepaymentLength;
				return (int)((float)settings.Amount * ((c * Mathf.Pow (1 + c, n)) / (Mathf.Pow (1 + c, n) - 1)));
			}
		}

		protected LoanSettings settings;
		protected int elapsedTime = 0;

		public Loan () {}

		public void AddTime () {
			elapsedTime ++;
			if (elapsedTime > settings.GracePeriod) {
				// TODO: handle negatives
				Player.Instance.Inventory[Group.ID].Remove (Payment);				
				Debug.Log ("repayment");
			}
			if (elapsedTime == settings.GracePeriod + settings.RepaymentLength) {
				Debug.Log ("Loan repaid");
			}
		}
	}

	public class Loan<T> : Loan where T : ItemGroup, new () {

		T group;

		public Loan () {
			settings = DataManager.GetLoanSettings (this.GetType ());
			group = new T () as T;
			group.Capacity = settings.Amount;
		}
	}
}