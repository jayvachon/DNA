using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GameInventory;
using GameActions;

namespace Units {

	public class GivingTreeUnit : StaticUnit, IActionPerformer {

		public override string Name {
			get { return "Giving Tree"; }
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

		int positionIndex = 0;

		void Awake () {

			Inventory = new Inventory (this);
			Inventory.Add (new CoffeeHolder (100, 35));
			Inventory.Add (new YearHolder (250, 0));
			Inventory.Get<YearHolder> ().HolderFilled += OnYearsCollected;
			Inventory.Get<YearHolder> ().DisplaySettings = new ItemHolderDisplaySettings (true, true);

			AcceptableActions = new AcceptableActions (this);
			AcceptableActions.Add (new AcceptDeliverItem<CoffeeHolder> ());
			AcceptableActions.Add (new AcceptDeliverAllYears ());

			PerformableActions = new PerformableActions (this);
			PerformableActions.Add (new GenerateUnit<Distributor, CoffeeHolder> (15, OnUnitGenerated), "Birth Laborer (15C)");
			PerformableActions.Add (new GenerateUnit<Elder, CoffeeHolder> (0, OnUnitGenerated), "Birth Elder (temp)");
			PerformableActions.Add (new GenerateUnit<Corpse, CoffeeHolder> (0, OnUnitGenerated), "Birth Corpse (temp)");
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

		void Update () {
			if (Input.GetKeyDown (KeyCode.Space)) {
				// PerformableActions.Print ();
			}
		}
	}
}