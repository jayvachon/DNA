using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using InventorySystem;
using DNA.Models;

namespace DNA {

	public abstract class Loan : Item {

		public delegate void OnUpdate ();

		public override string Name {
			get { return "Loan"; }
		}

		public abstract int Amount { get; }
		public abstract int Payment { get; }
		public abstract int Owed { get; }
		
		public string StatusDetails {
			get {
				switch (Status) {
					case LoanStatus.Grace: return "Grace: " + elapsedTime + "/" + settings.GracePeriod;
					case LoanStatus.Repayment: 
						return "Repayment: " + (elapsedTime-settings.GracePeriod) + "/" + (settings.RepaymentLength) + 
							((warningCount > 0)
							? "\nWarnings: " + warningCount + "/" + warningMax
							: "");
					case LoanStatus.Late: return "Warning " + warningCount + "/" + warningMax;
				}
				return "Repaid";
			}
		}

		public enum LoanStatus { Grace, Repayment, Late, Defaulted }
		public LoanStatus Status { get; private set; }

		protected LoanSettings settings;
		protected int elapsedTime = 0;
		int warningCount = 0;
		int warningMax = 3;

		public OnUpdate onUpdate;

		public Loan () {
			Status = LoanStatus.Grace;
			elapsedTime = 0;
			warningCount = 0;
			warningMax = 3;
		}

		public int IteratePayment () {
			elapsedTime ++;
			if (elapsedTime <= settings.GracePeriod)
				return 0;
			return Payment;
		}

		public abstract Repayment GetRepayment ();
		public abstract void ReturnRepayment (Repayment repayment);

		public class Repayment {

			public readonly int ID;
			public readonly string Type;
			public int Amount { get; set; }

			public Repayment (int id, string type, int amount) {
				ID = id;
				Type = type;
				Amount = amount;
			}
		}
	}

	public class Loan<T> : Loan where T : ItemGroup, new () {

		public override int Amount {
			get { return settings.Amount; }
		}

		public override int Payment {
			get {
				// https://upload.wikimedia.org/math/e/b/0/eb0d554c1ead49a1a0bd3155b764ec0c.png				
				float c = settings.InterestRate;
				float n = settings.RepaymentLength;
				return (int)((float)group.Capacity * ((c * Mathf.Pow (1 + c, n)) / (Mathf.Pow (1 + c, n) - 1)));
			}
		}

		public override int Owed {
			get {
				int repaymentsTotal = 0;
				foreach (var repayment in repayments)
					repaymentsTotal += repayment.Value.Amount;
				return group.Count + repaymentsTotal;
			}
		}

		T group;
		Dictionary<int, Repayment> repayments = new Dictionary<int, Repayment> ();
		int repaymentCounter = 0;

		public Loan () : base () {
			settings = DataManager.GetLoanSettings (this.GetType ());
			group = new T () { Capacity = settings.Amount };
			group.Fill ();
		}

		public override Repayment GetRepayment () {
			Repayment r = new Repayment (repaymentCounter, group.ID, group.Remove (Payment).Count);
			repayments.Add (repaymentCounter, r);
			repaymentCounter ++;
			return r;
		}

		public override void ReturnRepayment (Repayment repayment) {
			repayments.Remove (repayment.ID);
			group.Capacity = Mathf.Max (repayment.Amount + group.Count, group.Capacity);
			group.Add (repayment.Amount);
		}
	}
}