using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using InventorySystem;
using DNA.Tasks;

namespace DNA.Units {

	public class Shark : Unit, IDamageable {

		Lazer lazer;
		ProgressBar pbar;
		Loan loan;
		Loan.Repayment repayment;
		MatchResult givingTreeTask;
		BuildingIndicator indicator;
		Vector3 startPosition;
		GivingTreeUnit givingTree;

		string LoanType {
			get { return repayment.Type; }
		}

		public static Shark Create (Vector3 position, GivingTreeUnit givingTree, Loan loan) {
			Shark shark = UnitManager.Instantiate<Shark> (position);
			shark.Init (givingTree, loan);
			return shark;
		}

		void Init (GivingTreeUnit givingTree, Loan loan) {
			
			this.givingTree = givingTree;

			// Create progress bar
			if (pbar == null)
				pbar = UI.Instance.CreateProgressBar (MyTransform, new Vector3 (0, -0.5f, 0));
			pbar.SetProgress (Inventory["Health"].PercentFilled);

			// Create laser
			if (lazer == null)
				lazer = Lazer.Create (MyTransform);

			// Initialize inventory
			this.loan = loan;
			this.repayment = loan.GetRepayment ();
			Inventory[LoanType].Capacity = repayment.Amount;

			// Create indicator
			indicator = BuildingIndicator.Instantiate (LoanType == "Coffee" ? "coffee" : "derrick", MyTransform);

			// Set trajectory
			startPosition = Position;
			Vector3 targetPosition = givingTree.Position;
			Vector3 dir = (startPosition - targetPosition).normalized;
			targetPosition += dir * 3;
			startPosition.y += 2;
			targetPosition.y += 2;
			
			MyTransform.MoveTo (targetPosition, 1f, () => {
				if (!gameObject.activeSelf)
					return;
				lazer.StartFire (givingTree.MyTransform, new Vector3 (0, 2, 0));
				givingTree.StartTakeDamage ();
				givingTreeTask = TaskMatcher.StartMatch (this, givingTree, true);
			});
		}

		protected override void OnInitInventory (Inventory i) {

			// Initialize inventory with instruction to return to start after resources have been fetched
			i.Add (new CoffeeGroup (0, 0)).onFill += MoveToStart;
			i.Add (new MilkshakeGroup (0, 0)).onFill += MoveToStart;

			HealthGroup health = new HealthGroup (100, 100);
			health.onUpdate += () => {

				// Update the indicator as health changes
				pbar.SetProgress (Inventory["Health"].PercentFilled);
			};
			health.onEmpty += () => {

				// Stop the task if it's being performed, then return the payment
				if (givingTreeTask != null && givingTreeTask.Match.Performing) {
					givingTreeTask.Match.onEnd += (PerformerTask t) => {
						ReturnRepayment ();
						givingTreeTask.Match.onEnd = null;
					};
					givingTreeTask.Stop ();
				} else {
					ReturnRepayment ();
				}

			};

			i.Add (health);
		}

		void ReturnRepayment () {

			// Return the loan to recalculate amount owed
			loan.ReturnRepayment (repayment);
			Inventory[LoanType].TransferAll (givingTree.Inventory[LoanType]);

			// Return resources to the player and kill the shark
			DestroyThis<Shark> ();
		}

		void MoveToStart () {
			lazer.StopFire ();
			MyTransform.MoveTo (startPosition, 1f, () => {
				DestroyThis<Shark> ();
			});
		}

		protected override void OnInitPerformableTasks (PerformableTasks p) {
			p.Add (new ConsumeItem<HealthGroup> ());
			p.Add (new CollectItem<CoffeeGroup> ("shark_collect_coffee"));
			p.Add (new CollectItem<MilkshakeGroup> ("shark_collect_milkshake"));
		}

		protected override void OnEnable () {
			base.OnEnable ();
			Inventory["Health"].Fill ();
		}

		protected override void OnDisable () {
			base.OnDisable ();

			ObjectPool.Destroy<BuildingIndicator> (indicator);
			lazer.StopFire ();

			// Reset the inventory
			Inventory.Clear ();
			Inventory["Coffee"].Capacity = 0;
			Inventory["Milkshakes"].Capacity = 0;
			StopTakeDamage ();
		}

		public void StartTakeDamage () {
			PerformableTasks[typeof (ConsumeItem<HealthGroup>)].Start ();
		}

		public void StopTakeDamage () {
			PerformableTasks[typeof (ConsumeItem<HealthGroup>)].Stop ();
		}
	}
}