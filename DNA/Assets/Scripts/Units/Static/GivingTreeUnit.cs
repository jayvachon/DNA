#undef GENERATE_ALL
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DNA.Tasks;
using DNA.Paths;
using InventorySystem;

namespace DNA.Units {

	public class GivingTreeUnit : StaticUnit, ITaskPerformer {

		PerformableTasks performableTasks;
		public PerformableTasks PerformableTasks {
			get {
				if (performableTasks == null) {
					performableTasks = new PerformableTasks (this);
				}
				return performableTasks;
			}
		}

		List<Vector3> createPositions;
		List<Vector3> CreatePositions {
			get {
				if (createPositions == null) {
					createPositions = new List<Vector3> ();
					int positionCount = 8;
					float radius = 2;
					float deg = 360f / (float)positionCount;
					Vector3 center = UnitTransform.Position;//StaticTransform.Position;
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

		int positionIndex = 4;

		void Awake () {

			Inventory = Player.Instance.Inventory;

			PerformableTasks.Add (new GenerateUnit<Distributor> ()).onComplete += OnGenerateDistributor;
			PerformableTasks.Add (new GenerateUnit<Elder> ()).onComplete += OnGenerateElder;
			PerformableTasks.Add (new GenerateUnit<Corpse> ()).onComplete += OnGenerateCorpse;

		}

		protected override void OnInitAcceptableTasks (AcceptableTasks a) {
			a.Add (new AcceptDeliverItem<MilkshakeGroup> ());
			a.Add (new AcceptDeliverItem<CoffeeGroup> ());
		}

		void OnGenerateDistributor (PerformerTask task) {
			OnUnitGenerated (((GenerateUnit<Distributor>)task).GeneratedUnit);
		}

		void OnGenerateElder (PerformerTask task) {
			OnUnitGenerated (((GenerateUnit<Elder>)task).GeneratedUnit);
		}

		void OnGenerateCorpse (PerformerTask task) {
			OnUnitGenerated (((GenerateUnit<Corpse>)task).GeneratedUnit);
		}

		void OnUnitGenerated (Unit unit) {
			if (positionIndex >= CreatePositions.Count-1) {
				positionIndex = 0;
			} else {
				positionIndex ++;
			}
			((MobileUnit)unit).SetStartPoint ((GridPoint)Element);
		}

		void OnYearsCollected () {
			ChangeUnit<GivingTreeUnit, GivingTreeRipe> ();
		}
	}
}