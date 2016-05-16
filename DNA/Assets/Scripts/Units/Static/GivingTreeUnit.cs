#undef GENERATE_ALL
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DNA.Tasks;
using DNA.Paths;
using InventorySystem;

namespace DNA.Units {

	public class GivingTreeUnit : StaticUnit, IDamageable {//, ISeedProducer {

		List<Vector3> createPositions;
		List<Vector3> CreatePositions {
			get {
				if (createPositions == null) {
					createPositions = new List<Vector3> ();
					int positionCount = 8;
					float radius = 2;
					float deg = 360f / (float)positionCount;
					Vector3 center = UnitTransform.Position;
					for (int i = 0; i < positionCount; i ++) {
						float radians = (float)i * deg * Mathf.Deg2Rad;
						createPositions.Add (new Vector3 (
							center.x + radius * Mathf.Sin (radians),
							center.y,
							center.z + radius * Mathf.Cos (radians)
						));
					}
				}
				return createPositions;
			}
		}

		// SeedProductionHandler seedProduction;

		void Awake () {
			Inventory = Player.Instance.Inventory;
		}

		protected override void OnInitPerformableTasks (PerformableTasks p) {
			p.Add (new GenerateLaborer ()).onComplete += OnGenerateLaborer;
		}

		protected override void OnEnable () {
			base.OnEnable ();
			// StartSeedProduction ();
		}

		protected override void OnDisable () {
			base.OnDisable ();
			// seedProduction.Stop ();
		}

		protected override void OnInitAcceptableTasks (AcceptableTasks a) {
			// a.Add (new AcceptDeliverItem<MilkshakeGroup> ());
			// a.Add (new AcceptDeliverItem<CoffeeGroup> ());
			a.Add (new AcceptCollectItem<MilkshakeGroup> ());
			a.Add (new AcceptCollectItem<CoffeeGroup> ());
		}

		void OnGenerateLaborer (PerformerTask task) {
			OnUnitGenerated (((GenerateUnit<Laborer>)task).GeneratedUnit);
		}

		void OnGenerateElder (PerformerTask task) {
			OnUnitGenerated (((GenerateUnit<Elder>)task).GeneratedUnit);
		}

		void OnGenerateCorpse (PerformerTask task) {
			OnUnitGenerated (((GenerateUnit<Corpse>)task).GeneratedUnit);
		}

		void OnUnitGenerated (Unit unit) {
			((MobileUnit)unit).SetStartPoint ((GridPoint)Element);
		}

		void OnYearsCollected () {
			ChangeUnit<GivingTreeUnit, GivingTreeRipe> ();
		}

		public void StartSeedProduction () {
			// seedProduction = new SeedProductionHandler (MyTransform, 3.5f);
		}

		public void StartTakeDamage () {}
		public void StopTakeDamage () {}
	}
}