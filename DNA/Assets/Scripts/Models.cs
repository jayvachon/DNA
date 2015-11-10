using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using InventorySystem;
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

		UnitsSettings unitsSettings;
		public UnitsSettings UnitsSettings {
			get {
				if (unitsSettings == null) {
					unitsSettings = new UnitsSettings ();
				}
				return unitsSettings;
			}
		}
	}

	public class UnitsSettings {

		Dictionary<System.Type, UnitSettings> units;

		public UnitSettings this[System.Type unitType] {
			get { 
				try {
					return units[unitType]; 
				} catch {
					throw new System.Exception ("Could not find a model for '" + unitType + "'");
				}
			}
		}

		public UnitsSettings () {

			units = new Dictionary<System.Type, UnitSettings> ();

			units.Add (typeof (Distributor), new UnitSettings {
				Symbol = "laborer",
				Title = "Laborer",
				Description = "Laborers perform work until they reach retirement age.",
				Emissions = 0.01f
			});

			units.Add (typeof (Elder), new UnitSettings {
				Symbol = "elder",
				Title = "Elder",
				Description = "Elders need to be cared for.",
				Emissions = 0.01f
			});

			units.Add (typeof (Corpse), new UnitSettings {
				Symbol = "corpse",
				Title = "Remains",
				Description = "Deliver remains to the Giving Tree to harvest the years.",
				Emissions = 0f
			});

			units.Add (typeof (MilkshakePool), new UnitSettings {
				Symbol = "derrick",
				Title = "Milkshake Derrick",
				Description = "Milkshakes collected from a Derrick can be used to construct buildings.",
				Emissions = 1f,
				TakesDamage = true
			});

			units.Add (typeof (CoffeePlant), new UnitSettings {
				Symbol = "coffee",
				Title = "Coffee Plant",
				Description = "Deliver coffee to the Giving Tree to create more Laborers.",
				Emissions = -0.01f,
				TakesDamage = true
			});

			units.Add (typeof (University), new UnitSettings {
				Symbol = "university",
				Title = "University",
				Description = "Upgrade units by conducting research at the University.",
				Emissions = 0.5f,
				TakesDamage = true
			});

			units.Add (typeof (Clinic), new UnitSettings {
				Symbol = "clinic",
				Title = "Clinic",
				Description = "Elders live longer when they're receiving care at a Clinic.",
				Emissions = 0.75f,
				TakesDamage = true
			});

			units.Add (typeof (DrillablePlot), new UnitSettings {
				Symbol = "plot",
				Title = "Plot",
				Description = "This plot can be drilled for milkshakes.",
				Emissions = 0f,
				TakesDamage = false
			});

			units.Add (typeof (GivingTreeUnit), new UnitSettings {
				Symbol = "tree",
				Title = "Giving Tree",
				Description = "The Giving Tree gives birth to Laborers and is also a portal to the next dimension.",
				Emissions = 0f,
				TakesDamage = false
			});

			units.Add (typeof (ConstructionSite), new UnitSettings {
				Symbol = "construction",
				Title = "Construction Site",
				Description = "A building to be.",
				Emissions = 0f,
				TakesDamage = false
			});

			units.Add (typeof (RepairSite), new UnitSettings {
				Symbol = "repair",
				Title = "Repair Site",
				Description = "A damaged building that needs repairs.",
				Emissions = 0f,
				TakesDamage = false
			});
		}
	}

	// TODO: this should be grabbed from a json file, but just doing it here for now
	public class TasksSettings {
		
		Dictionary<System.Type, TaskSettings> tasks;

		public Dictionary<System.Type, TaskSettings> Tasks {
			get { return tasks; }
		}

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

			tasks.Add (typeof (CollectItem<MilkshakeGroup>), new TaskSettings {
				Symbol = "collect_milkshake",
				Duration = 2f,
				AutoStart = false,
				Repeat = true,
				Pair = typeof (AcceptDeliverItem<MilkshakeGroup>)
			});

			tasks.Add (typeof (CollectItem<CoffeeGroup>), new TaskSettings {
				Symbol = "collect_coffee",
				Duration = 1.5f,
				AutoStart = false,
				Repeat = true,
				Pair = typeof (AcceptDeliverItem<CoffeeGroup>)
			});

			tasks.Add (typeof (CollectItem<LaborGroup>), new TaskSettings {
				Symbol = "collect_labor",
				Duration = 1f,
				AutoStart = false,
				Repeat = true
			});

			tasks.Add (typeof (CollectItem<HealthGroup>), new TaskSettings {
				Symbol = "collect_health",
				Duration = 2f,
				AutoStart = false,
				Repeat = true
			});

			/**
			 *	Construct
			 */

			tasks.Add (typeof (ConstructUnit<CoffeePlant>), new CostTaskSettings {
				Symbol = "construct_coffee",
				Title = "Birth Coffee Plant (20M)",
				Description = "Creates a new coffee plant",
				Duration = 0f,
				Costs = new [] {
					new Dictionary<string, int> {
						{ "Milkshakes", 20 }
					}
				}
			});

			tasks.Add (typeof (ConstructUnit<MilkshakePool>), new CostTaskSettings {
				Symbol = "construct_derrick",
				Title = "Birth Milkshake Derrick (35M)",
				Description = "Creates a new milkshake derrick",
				Duration = 0f,
				Costs = new [] {
					new Dictionary<string, int> {
						{ "Milkshakes", 20 }
					}
				}
			});

			tasks.Add (typeof (ConstructUnit<University>), new CostTaskSettings {
				Symbol = "construct_university",
				Title = "Birth University (50M)",
				Description = "Creates a new university",
				Duration = 0f,
				Costs = new [] {
					new Dictionary<string, int> {
						{ "Milkshakes", 50 }
					}
				}
			});

			tasks.Add (typeof (ConstructUnit<Clinic>), new CostTaskSettings {
				Symbol = "construct_clinic",
				Title = "Birth Clinic (40M)",
				Description = "Creates a new Clinic",
				Duration = 0f,
				Costs = new [] {
					new Dictionary<string, int> {
						{ "Milkshakes", 40 }
					}
				}
			});

			/**
			 *	Consume
			 */

			tasks.Add (typeof (ConsumeItem<YearGroup>), new TaskSettings {
				Symbol = "construct_year",
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

			tasks.Add (typeof (DeliverItem<MilkshakeGroup>), new TaskSettings {
				Symbol = "deliver_milkshake",
				Title = "",
				Description = "Delivers milkshakes",
				Duration = 2f,
				AutoStart = false,
				Repeat = true,
				Pair = typeof (AcceptCollectItem<MilkshakeGroup>)
			});

			tasks.Add (typeof (DeliverItem<CoffeeGroup>), new TaskSettings {
				Symbol = "construct_coffee",
				Title = "",
				Description = "Delivers coffee",
				Duration = 1.5f,
				AutoStart = false,
				Repeat = true,
				Pair = typeof (AcceptCollectItem<CoffeeGroup>)
			});

			/**
			 *	GenerateItem
			 */

			tasks.Add (typeof (GenerateItem<CoffeeGroup>), new TaskSettings {
				Symbol = "generate_coffee",
				Duration = 2f,
				AutoStart = true,
				Repeat = true,
				Pair = null
			});

			tasks.Add (typeof (GenerateItem<YearGroup>), new TaskSettings {
				Symbol = "generate_year",
				Duration = 1f,
				AutoStart = true,
				Repeat = true,
				Pair = null
			});

			tasks.Add (typeof (GenerateItem<HealthGroup>), new TaskSettings {
				Symbol = "generate_health",
				Duration = 5f,
				AutoStart = true,
				Repeat = true,
				Pair = null
			});

			/**
			 *	GenerateUnit
			 */

			tasks.Add (typeof (GenerateUnit<Distributor>), new CostTaskSettings {
				Symbol = "generate_laborer",
				Title = "Birth Laborer (15C)",
				Description = "Creates a new laborer",
				Duration = 0f,
				AutoStart = false,
				Repeat = false,
				Pair = null,
				Costs = new [] {
					new Dictionary<string, int> {
						{ "Coffee", 15 }
					}
				}
			});

			/**
			 *	ResearchUpgrade
			 */

			tasks.Add (typeof (ResearchUpgrade<CoffeeCapacity>), new CostTaskSettings {
				Symbol = "reseach_coffee",
				Title = "Upgrade Laborer coffee capacity",
				Description = "Laborers will be able to carry more coffee",
				AutoStart = false,
				Repeat = false,
				Costs = new [] {
					new Dictionary<string, int> {
						{ "Milkshakes", 50 }
					},
					new Dictionary<string, int> {
						{ "Milkshakes", 100 }
					},
					new Dictionary<string, int> {
						{ "Milkshakes", 200 }
					}
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
				Costs = new [] {
					new Dictionary<string, int> {
						{ "Milkshakes", 1 }
					}
				}
			});

			tasks.Add (typeof (ConstructRoad), new CostTaskSettings {
				Title = "Birth road",
				Description = "Builds a road",
				Duration = 0f,
				AutoStart = false,
				Repeat = false,
				Pair = null,
				Costs = new [] {
					new Dictionary<string, int> {
						{ "Milkshakes", 20 }
					}
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

			tasks.Add (typeof (GenerateItemTest<MilkshakeGroup>), new TaskSettings {
				Title = "Generate Milkshakes",
				Description = "Creates a milkshake",
				Duration = 0.1f,
				AutoStart = false,
				Repeat = true,
				Pair = null
			});

			tasks.Add (typeof (ConsumeItemTest<CoffeeGroup>), new TaskSettings {
				Title = "Consume Coffee",
				Description = "Destroys a coffee",
				Duration = 0.1f,
				AutoStart = false,
				Repeat = true,
				Pair = null
			});

			tasks.Add (typeof (CollectItemTest<YearGroup>), new TaskSettings {
				Title = "Collect happiness",
				Description = "Collects a happiness from an acceptor",
				Duration = 0.1f,
				AutoStart = false,
				Repeat = true,
				Pair = typeof (AcceptDeliverItemTest<YearGroup>)
			});

			tasks.Add (typeof (DeliverItemTest<YearGroup>), new TaskSettings {
				Title = "Deliver Happiness",
				Description = "Delivers a happiness to an acceptor",
				Duration = 0.1f,
				AutoStart = false,
				Repeat = true,
				Pair = typeof (AcceptCollectItemTest<YearGroup>)
			});

			tasks.Add (typeof (GenerateUnitTest<Distributor>), new CostTaskSettings {
				Title = "Generate Distributor",
				Description = "Generates a distributor",
				Duration = 0f,
				AutoStart = false,
				Repeat = false,
				Pair = null,
				Costs = new [] {
					new Dictionary<string, int> {
						{ "Milkshakes", 2 },
						{ "Coffee", 1 }
					}
				}
			});
		}
	}

	public class TaskSettings {
		public string Symbol { get; set; }
		public string Title { get; set; }
		public string Description { get; set; }
		public System.Type Pair { get; set; }
		public float Duration { get; set; }
		public bool AutoStart { get; set; }
		public bool Repeat { get; set; }
	}

	public class CostTaskSettings : TaskSettings {

		// An array of costs. The CostTask will use the Costs at the first array position, then iterate the array if there are more elements.
		// This is useful for e.g. upgrades
		// <ItemGroup ID, amount required>
		public Dictionary<string, int>[] Costs { get; set; }
	}

	public class UnitSettings {
		public string Symbol { get; set; }
		public string Title { get; set; }
		public string Description { get; set; }
		public float Emissions { get; set; }
		public bool TakesDamage { get; set; }
	}
}
