#undef GENERATE_ALL
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GameInventory;
using GameActions;
using DNA.Tasks;

namespace Units {

	public class GivingTreeUnit : StaticUnit, IActionPerformer {

		public override string Name {
			get { return "Giving Tree"; }
		}

		public override string Description {
			get { return "The Giving Tree gives birth to Laborers and is also a portal to the next dimension."; }
		}

		public PerformableActions PerformableActions { get; private set; }

		List<Vector3> createPositions;
		List<Vector3> CreatePositions {
			get {
				if (createPositions == null) {
					createPositions = new List<Vector3> ();
					int positionCount = 8;
					float radius = 2;
					float deg = 360f / (float)positionCount;
					Vector3 center = StaticTransform.Position;
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

		/*public Inventory Inventory {
			get { return Player.Instance.Inventory; }
		}*/		

		void Awake () {

			Inventory = new Inventory (this);
			Inventory.Add (new CoffeeHolder (100, 50));
			Inventory.Add (new MilkshakeHolder (100000, 30));
			Inventory.Add (new YearHolder (350, 0));
			Inventory.Get<YearHolder> ().HolderFilled += OnYearsCollected;
			Inventory.Get<YearHolder> ().DisplaySettings = new ItemHolderDisplaySettings (true, true);
			Inventory.Get<MilkshakeHolder> ().DisplaySettings = new ItemHolderDisplaySettings (true, false);

			/*AcceptableActions = new AcceptableActions (this);
			//AcceptableActions.Add (new AcceptDeliverItem<CoffeeHolder> ());
			AcceptableActions.Add (new AcceptDeliverToPlayer<MilkshakeHolder> ());
			AcceptableActions.Add (new AcceptCollectItem<MilkshakeHolder> ());
			//AcceptableActions.Add (new AcceptDeliverAllYears ());*/
			AcceptableTasks.Add (new DNA.Tasks.AcceptDeliverItem<MilkshakeHolder> ());

			PerformableActions = new PerformableActions (this);
			PerformableActions.OnStartAction += OnStartAction;
			PerformableActions.Add (new GenerateUnit<Distributor, CoffeeHolder> (-1, OnUnitGenerated), "Birth Laborer (15C)");
			#if GENERATE_ALL
			PerformableActions.Add (new GenerateUnit<Elder, CoffeeHolder> (0, OnUnitGenerated), "Birth Elder (temp)");
			PerformableActions.Add (new GenerateUnit<Corpse, CoffeeHolder> (0, OnUnitGenerated), "Birth Corpse (temp)");
			#endif
		}

		void OnStartAction (string id) {
			if (id == "GenerateDistributor") {
				//PerformableActions.SetActive ("GenerateDistributor", false);
			}
		}

		void OnUnitGenerated (Unit unit) {
			unit.Position = CreatePositions[positionIndex];
			if (positionIndex >= CreatePositions.Count-1) {
				positionIndex = 0;
			} else {
				positionIndex ++;
			}
			((MobileUnit)unit).Init (PathPoint);
			//((MobileUnit)unit).Init (this);
			//PerformableActions.SetActive ("GenerateDistributor", true);
			//RefreshInfoContent ();
		}

		void OnYearsCollected () {
			ChangeUnit<GivingTreeUnit, GivingTreeRipe> ();
		}
	}
}