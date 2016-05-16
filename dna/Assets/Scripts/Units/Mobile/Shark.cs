using UnityEngine;
using System.Collections;
using InventorySystem;
using DNA.Tasks;

namespace DNA.Units {

	public class Shark : Unit, IDamageable {

		Lazer lazer;
		ProgressBar pbar;
		Loan loan;
		Loan.Repayment repayment;
		MatchResult givingTreeTask;

		public static Shark Create (Vector3 position, GivingTreeUnit givingTree, Loan loan) {
			Shark shark = ObjectPool.Instantiate<Shark> (position);
			shark.Init (givingTree, loan);
			return shark;
		}

		void Init (GivingTreeUnit givingTree, Loan loan) {
			
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
			Inventory[repayment.Type].Capacity = repayment.Amount;

			// Set trajectory
			Vector3 startPosition = Position;
			Vector3 targetPosition = givingTree.Position;
			Vector3 dir = (startPosition - targetPosition).normalized;
			targetPosition += dir * 3;
			startPosition.y += 2;
			targetPosition.y += 2;
			
			float distance = Vector3.Distance (startPosition, targetPosition);
			float speed = 5f;

			// Move
			Co2.StartCoroutine (distance / speed, (float p) => {
				Position = Vector3.Lerp (startPosition, targetPosition, p);
				MyTransform.LookAt (targetPosition);
			}, () => {
				lazer.StartFire (givingTree.MyTransform, new Vector3 (0, 2, 0));
				givingTree.StartTakeDamage ();
				givingTreeTask = TaskMatcher.GetPerformable (this, givingTree);
				givingTreeTask.Start (true);
			});
		}

		protected override void OnInitInventory (Inventory i) {

			i.Add (new CoffeeGroup (0, 0)).onFill += () => {
				Debug.Log ("coffee full");
			};

			i.Add (new MilkshakeGroup (0, 0)).onFill += () => {
				Debug.Log ("milk full");
			};

			HealthGroup health = new HealthGroup (100, 100);
			health.onUpdate += () => {
				pbar.SetProgress (Inventory["Health"].PercentFilled);
			};
			health.onEmpty += () => {
				loan.ReturnRepayment (repayment);
				DestroyThis<Shark> ();
			};

			i.Add (health);
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
			lazer.StopFire ();
			givingTreeTask.Stop ();
			Inventory.Clear ();
			Inventory["Coffee"].Capacity = 0;
			Inventory["Milkshakes"].Capacity = 0;
		}

		public void StartTakeDamage () {
			PerformableTasks[typeof (ConsumeItem<HealthGroup>)].Start ();
		}

		public void StopTakeDamage () {
			PerformableTasks[typeof (ConsumeItem<HealthGroup>)].Stop ();
		}
	}
}