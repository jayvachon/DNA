using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using InventorySystem;

namespace DNA {

	public static class LoanManager {

		public delegate void OnUpdateLoans ();

		static float repaymentTime = 60f;

		static Inventory inventory;
		public static Inventory Inventory {
			get {
				if (inventory == null) {
					inventory = new Inventory ();
					inventory.Add (new MilkshakeLoanGroup (3)).onUpdate += OnUpdate;
					inventory.Add (new CoffeeLoanGroup (3)).onUpdate += OnUpdate;
					Co.Start (repaymentTime, OnElapseTime, ElapseTime);
				}
				return inventory;
			}
		}

		static bool defaulted = false;
		public static bool Defaulted {
			get { return defaulted; }
			set { defaulted = value; }
		}

		public static float Time { get; private set; }
		public static OnUpdateLoans onUpdateLoans;

		static void OnElapseTime (float t) {
			Time = t;
		}

		static void ElapseTime () {

			foreach (var group in Inventory.Groups) {
				foreach (Item item in group.Value.Items) {
					((Loan)item).AddTime ();
				}
			}

			Co.Start (repaymentTime, OnElapseTime, ElapseTime);
		}

		static void OnUpdate () {
			if (onUpdateLoans != null)
				onUpdateLoans ();
		}
	}
}