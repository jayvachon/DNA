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

		public float PercentUnpaid {
			get { return Owed / Amount; }
		}
		
		protected LoanSettings settings;
		public OnUpdate onUpdate;

		public abstract Repayment GetRepayment ();
		public abstract void ReturnRepayment (Repayment repayment);

		protected int CalculatePayment (float principal, float interestRate, float time) {
			return Mathf.RoundToInt ((interestRate * principal) / (1 - Mathf.Pow (1 + interestRate, -time)));
		}

		protected int CalculateOwed (float principal, float interestRate, float time) {
			return Mathf.RoundToInt (CalculatePayment (principal, interestRate, time) * time);
		}

		public class Repayment {

			public readonly int ID;
			public readonly string Type;
			public readonly int Interest;
			public int Amount { get; set; }

			public Repayment (int id, string type, int amount, int interest) {
				ID = id;
				Type = type;
				Amount = amount;
				Interest = interest;
			}

			public override string ToString () {
				return "id: " + ID + "\nType: " + Type + "\nAmount: " + Amount;
			}
		}
	}

	public class Loan<T> : Loan where T : ItemGroup, new () {

		// The original principal
		public override int Amount {
			get { return settings.Amount; }
		}

		// Amount owed in a single payment
		public override int Payment {
			get {
				return CalculatePayment (principal, settings.InterestRate, settings.RepaymentLength);
			}
		}

		// Total amount owed
		public override int Owed {
			get {
				return CalculateOwed (principal, settings.InterestRate, settings.RepaymentLength);
			}
		}

		// Dynamical principal (updated when payments are missed)
		int principal;

		T group;
		Dictionary<int, Repayment> repayments = new Dictionary<int, Repayment> ();
		int repaymentCounter = 0;

		public Loan () : base () {
			settings = DataManager.GetLoanSettings (this.GetType ());
			principal = settings.Amount;
			group = new T () { Capacity = Owed };
			group.Fill ();
		}

		public override Repayment GetRepayment () {

			// Calculate how much of the payment is interest
			int interest = Payment - (int)(principal/settings.RepaymentLength);

			// Create the repayment, track it, and return it
			Repayment r = new Repayment (repaymentCounter, group.ID, group.Remove (Payment).Count, interest);
			repayments.Add (repaymentCounter, r);
			repaymentCounter ++;
			return r;
		}

		public override void ReturnRepayment (Repayment repayment) {
			
			// (Payment was missed)

			// Remove the repayment and add the interest to the principal
			repayments.Remove (repayment.ID);
			principal += repayment.Interest;

			// Update the capacity and add the resources back into the loan
			group.Capacity = Mathf.Max (repayment.Amount + group.Count, principal);
			group.Add (repayment.Amount);
		}

		public override string ToString () {
			return "Original principal: " + Amount + "\nPrincipal: " + principal + "\nOwed: " + Owed + "\nPayment: "  + Payment;
		}
	}
}