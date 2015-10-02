using UnityEngine;
using System.Collections;
using DNA.InventorySystem;
using DNA.Tasks;
using DNA.Paths;

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
				inventory.Add (new MilkshakeHolder (100000, 30));
				inventory.Add (new CoffeeHolder (100000, 50));
				inventory.Add (new YearHolder (5, 0));
			}
			return inventory;
		}
	}

	public MilkshakeHolder Milkshakes {
		get { return (MilkshakeHolder)Inventory["Milkshakes"]; }
	}

	PerformableTasks performableTasks;
	public PerformableTasks PerformableTasks {
		get {
			if (performableTasks == null) {
				performableTasks = new PerformableTasks (this);
				performableTasks.Add (new BuildRoad ());
			}
			return performableTasks;
		}
	}
}
