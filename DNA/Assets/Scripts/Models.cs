using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GameInventory;
using Units;
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
			
			tasks.Add (typeof (CollectItem<MilkshakeHolder>), new TaskSettings {
				Title = "Collect Milkshakes",
				Description = "Collects milkshakes",
				Duration = 0.5f,
				AutoStart = false,
				Repeat = true,
				Pair = typeof (AcceptDeliverItem<MilkshakeHolder>)
			});

			tasks.Add (typeof (DeliverItem<MilkshakeHolder>), new TaskSettings {
				Title = "Deliver Milkshakes",
				Description = "Delivers milkshakes",
				Duration = 0.5f,
				AutoStart = false,
				Repeat = true,
				Pair = typeof (AcceptCollectItem<MilkshakeHolder>)
			});

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
