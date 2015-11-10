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

			unitRenderer.SetColors (new Color (0.808f, 0.945f, 0.604f));

			Inventory = Player.Instance.Inventory;

			AcceptableTasks.Add (new AcceptDeliverItem<MilkshakeGroup> ());
			AcceptableTasks.Add (new AcceptDeliverItem<CoffeeGroup> ());

			PerformableTasks.Add (new GenerateUnit<Distributor> ()).onComplete += OnGenerateDistributor;

			/*PerformableActions = new PerformableActions (this);
			PerformableActions.OnStartAction += OnStartAction;
			PerformableActions.Add (new GenerateUnit<Distributor, CoffeeHolder> (-1, OnUnitGenerated), "Birth Laborer (15C)");
			#if GENERATE_ALL
			PerformableActions.Add (new GenerateUnit<Elder, CoffeeHolder> (0, OnUnitGenerated), "Birth Elder (temp)");
			PerformableActions.Add (new GenerateUnit<Corpse, CoffeeHolder> (0, OnUnitGenerated), "Birth Corpse (temp)");
			#endif*/
		}

		void OnGenerateDistributor (PerformerTask task) {
			Unit unit = ((GenerateUnit<Distributor>)task).GeneratedUnit;
			if (positionIndex >= CreatePositions.Count-1) {
				positionIndex = 0;
			} else {
				positionIndex ++;
			}
			((MobileUnit)unit).SetStartPoint ((GridPoint)Element);
		}

		void OnUnitGenerated (Unit unit) {
			unit.Position = CreatePositions[positionIndex];
			if (positionIndex >= CreatePositions.Count-1) {
				positionIndex = 0;
			} else {
				positionIndex ++;
			}
		}

		void OnYearsCollected () {
			ChangeUnit<GivingTreeUnit, GivingTreeRipe> ();
		}
	}
}