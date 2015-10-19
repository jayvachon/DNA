using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DNA.InventorySystem;
using DNA.Units;
using DNA.Tasks;

namespace DNA.Models {

	public class GameData {
		TasksSettings tasksSettings;
		public TasksSettings TasksSettings {
			get {
				if (tasksSettings == null) {
					tasksSettings = new TasksSettings ();
				}
				return tasksSettings;
			}
		}
	}

	// TODO: this should be grabbed from a json file, but just doing it here for now
	public class TasksSettings {
		
		Dictionary<System.Type, TaskSettings> tasks;

		public TaskSettings this[System.Type taskType] {
			get { 
				try {
					return tasks[taskType]; 
				} catch {
					throw new System.Exception ("Could not find a model for '" + taskType + "'");
				}
			}
		}

		public TasksSettings () {
			
			tasks = new Dictionary<System.Type, TaskSettings> ();
			
			/**
			 *	CollectItem
			 */

			tasks.Add (typeof (CollectItem<MilkshakeHolder>), new TaskSettings {
				Title = "",
				Description = "Collects milkshakes",
				Duration = 0.5f,
				AutoStart = false,
				Repeat = true,
				Pair = typeof (AcceptDeliverItem<MilkshakeHolder>)
			});

			tasks.Add (typeof (CollectItem<CoffeeHolder>), new TaskSettings {
				Title = "",
				Description = "Collects coffee",
				Duration = 0.5f,
				AutoStart = false,
				Repeat = true,
				Pair = typeof (AcceptDeliverItem<CoffeeHolder>)
			});

			tasks.Add (typeof (CollectItem<LaborHolder>), new TaskSettings {
				Title = "",
				Description = "Collects labor",
				Duration = 1f,
				AutoStart = false,
				Repeat = true,
				Pair = null
			});

			/**
			 *	Construct
			 */

			tasks.Add (typeof (ConstructUnit<CoffeePlant>), new CostTaskSettings {
				Title = "Birth Coffee Plant (10M)",
				Description = "Creates a new coffee plant",
				Duration = 0f,
				AutoStart = false,
				Repeat = false,
				Pair = null,
				Costs = new Dictionary<string, int> {
					{ "Milkshakes", 20 }
				}
			});

			tasks.Add (typeof (ConstructUnit<MilkshakePool>), new CostTaskSettings {
				Title = "Birth Milkshake Derrick (15M)",
				Description = "Creates a new milkshake derrick",
				Duration = 0f,
				AutoStart = false,
				Repeat = false,
				Pair = null,
				Costs = new Dictionary<string, int> {
					{ "Milkshakes", 35 }
				}
			});

			/**
			 *	Consume
			 */

			tasks.Add (typeof (ConsumeItem<YearHolder>), new TaskSettings {
				Title = "",
				Description = "Consumes year",
				Duration = 1f,
				AutoStart = false,
				Repeat = true,
				Pair = null
			});

			/**
			 *	DeliverItem
			 */

			tasks.Add (typeof (DeliverItem<MilkshakeHolder>), new TaskSettings {
				Title = "",
				Description = "Delivers milkshakes",
				Duration = 0.5f,
				AutoStart = false,
				Repeat = true,
				Pair = typeof (AcceptCollectItem<MilkshakeHolder>)
			});

			tasks.Add (typeof (DeliverItem<CoffeeHolder>), new TaskSettings {
				Title = "",
				Description = "Delivers coffee",
				Duration = 0.5f,
				AutoStart = false,
				Repeat = true,
				Pair = typeof (AcceptCollectItem<CoffeeHolder>)
			});

			/**
			 *	GenerateItem
			 */

			tasks.Add (typeof (GenerateItem<CoffeeHolder>), new TaskSettings {
				Title = "",
				Description = "Generates coffee",
				Duration = 1.5f,
				AutoStart = true,
				Repeat = true,
				Pair = null
			});

			/**
			 *	GenerateUnit
			 */

			tasks.Add (typeof (GenerateUnit<MilkshakePool>), new CostTaskSettings {
				Title = "Birth Milkshake Derrick (15M)",
				Description = "Creates a new milkshake derrick",
				Duration = 0f,
				AutoStart = false,
				Repeat = false,
				Pair = null,
				Costs = new Dictionary<string, int> {
					{ "Milkshakes", 15 }
				}
			});

			tasks.Add (typeof (GenerateUnit<CoffeePlant>), new CostTaskSettings {
				Title = "Birth Coffee Plant (10M)",
				Description = "Creates a new coffee plant",
				Duration = 0f,
				AutoStart = false,
				Repeat = false,
				Pair = null,
				Costs = new Dictionary<string, int> {
					{ "Milkshakes", 10 }
				}
			});

			tasks.Add (typeof (GenerateUnit<Distributor>), new CostTaskSettings {
				Title = "Birth Laborer (15C)",
				Description = "Creates a new laborer",
				Duration = 0f,
				AutoStart = false,
				Repeat = false,
				Pair = null,
				Costs = new Dictionary<string, int> {
					{ "Coffee", 15 }
				}
			});

			/**
			 *	Misc
			 */

			tasks.Add (typeof (FleeTree), new CostTaskSettings {
				Title = "Flee Tree",
				Description = "Goes to the next level",
				Duration = 0f,
				AutoStart = false,
				Repeat = false,
				Pair = null,
				Costs = new Dictionary<string, int> {
					{ "Milkshakes", 1 }
				}
			});

			tasks.Add (typeof (ConstructRoad), new CostTaskSettings {
				Title = "Build road",
				Description = "Builds a road",
				Duration = 0f,
				AutoStart = false,
				Repeat = false,
				Pair = null,
				Costs = new Dictionary<string, int> {
					{ "Milkshakes", 20 }
				}
			});

			/**
			 *	Tests
			 */

			tasks.Add (typeof (AutoStartTest), new TaskSettings {
				Title = "Auto Start Test",
				Description = "Tests to see if the action starts by itself upon creation",
				Duration = 0.1f,
				AutoStart = true,
				Repeat = false,
				Pair = null
			});

			tasks.Add (typeof (RepeatTest), new TaskSettings {
				Title = "Repeat Test",
				Description = "Tests to see if the action repeats itself upon ending",
				Duration = 1f,
				AutoStart = false,
				Repeat = true,
				Pair = null
			});

			tasks.Add (typeof (EnabledTest), new TaskSettings {
				Title = "Enabled Test",
				Description = "Tests to see if the action behaves correctly based on its enabled state",
				Duration = 1f,
				AutoStart = false,
				Repeat = false,
				Pair = null
			});

			tasks.Add (typeof (GenerateItemTest<MilkshakeHolder>), new TaskSettings {
				Title = "Generate Milkshakes",
				Description = "Creates a milkshake",
				Duration = 0.1f,
				AutoStart = false,
				Repeat = true,
				Pair = null
			});

			tasks.Add (typeof (ConsumeItemTest<CoffeeHolder>), new TaskSettings {
				Title = "Consume Coffee",
				Description = "Destroys a coffee",
				Duration = 0.1f,
				AutoStart = false,
				Repeat = true,
				Pair = null
			});

			tasks.Add (typeof (CollectItemTest<YearHolder>), new TaskSettings {
				Title = "Collect happiness",
				Description = "Collects a happiness from an acceptor",
				Duration = 0.1f,
				AutoStart = false,
				Repeat = true,
				Pair = typeof (AcceptDeliverItemTest<YearHolder>)
			});

			tasks.Add (typeof (DeliverItemTest<YearHolder>), new TaskSettings {
				Title = "Deliver Happiness",
				Description = "Delivers a happiness to an acceptor",
				Duration = 0.1f,
				AutoStart = false,
				Repeat = true,
				Pair = typeof (AcceptCollectItemTest<YearHolder>)
			});

			tasks.Add (typeof (GenerateUnitTest<Distributor>), new CostTaskSettings {
				Title = "Generate Distributor",
				Description = "Generates a distributor",
				Duration = 0f,
				AutoStart = false,
				Repeat = false,
				Pair = null,
				Costs = new Dictionary<string, int> {
					{ "Milkshakes", 1 },
					{ "Coffee", 2 }
				}
			});
		}
	}

	public class TaskSettings {
		public string Title { get; set; }
		public string Description { get; set; }
		public System.Type Pair { get; set; }
		public float Duration { get; set; }
		public bool AutoStart { get; set; }
		public bool Repeat { get; set; }
	}

	public class CostTaskSettings : TaskSettings {

		// ItemHolder ID, amount required
		public Dictionary<string, int> Costs { get; set; }
	}
}
