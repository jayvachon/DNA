using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DNA.InputSystem;
using DNA.Tasks;
using DNA.Paths;
using DNA.Units;
using InventorySystem;

namespace DNA {

	public class Player : MonoBehaviour, IInventoryHolder, ITaskPerformer {

		static Player instance = null;
		static public Player Instance {
			get {
				if (instance == null) {
					instance = Object.FindObjectOfType (typeof (Player)) as Player;
				}
				return instance;
			}
		}

		Inventory inventory;
		public Inventory Inventory {
			get {
				if (inventory == null) {
					inventory = new Inventory (this);
					Inventory.Add (new MilkshakeGroup ());
					Inventory.Add (new CoffeeGroup ());
					Inventory.Add (new YearGroup ());
					Inventory.Add (new LaborGroup ());
				}
				return inventory;
			}
		}

		public MilkshakeGroup Milkshakes {
			get { return Inventory.Get<MilkshakeGroup> (); }
		}

		PerformableTasks performableTasks;
		public PerformableTasks PerformableTasks {
			get {
				if (performableTasks == null) {
					performableTasks = new PerformableTasks (this);
					performableTasks.Add (new ConstructRoad ());
					performableTasks.Add (new ConstructUnit<Flower> ());
					performableTasks.Add (new ConstructUnit<MilkshakePool> ());
					performableTasks.Add (new ConstructUnit<University> ());
					// performableTasks.Add (new ConstructUnit<Clinic> ());
					performableTasks.Add (new ConstructUnit<CollectionCenter> ());
				}
				return performableTasks;
			}
		}

		void OnEmptyClick (System.Type type) {
			PlayerActionState.Set (ActionState.Idle);
		}
	}
}