using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using InventorySystem;
using DNA.Units;
using DNA.Tasks;
using DNA.Paths;

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

		LoansSettings loansSettings;
		public LoansSettings LoansSettings {
			get {
				if (loansSettings == null) {
					loansSettings = new LoansSettings ();
				}
				return loansSettings;
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

			units.Add (typeof (Road), new UnitSettings {
				Symbol = "road",
				Title = "Road",
				Description = "Roads connect buildings so that laborers can reach them.",
				Emissions = 0.01f,
				RemovesFogOfWar = true,
				Unlocked = true
			});

			units.Add (typeof (Laborer), new UnitSettings {
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
				TakesDamage = true,
				Unlocked = true
			});

			units.Add (typeof (CoffeePlant), new UnitSettings {
				Symbol = "coffee",
				Title = "Coffee Plant",
				Description = "Deliver coffee to the Giving Tree to create more laborers.",
				Emissions = 0f,
				TakesDamage = false,
				Unlocked = false
			});

			units.Add (typeof (University), new UnitSettings {
				Symbol = "university",
				Title = "University",
				Description = "Upgrade units by conducting research at the University.",
				Emissions = 0.5f,
				TakesDamage = true,
				Unlocked = true
			});

			units.Add (typeof (Clinic), new UnitSettings {
				Symbol = "clinic",
				Title = "Clinic",
				Description = "Elders live longer when they're receiving care at a Clinic.",
				Emissions = 0.75f,
				TakesDamage = true,
				Unlocked = false
			});

			units.Add (typeof (Flower), new UnitSettings {
				Symbol = "flower",
				Title = "Flower",
				Description = "Flowers are really pretty :)",
				Emissions = -0.05f,
				TakesDamage = true,
				RemovesFogOfWar = true
			});

			units.Add (typeof (CollectionCenter), new UnitSettings {
				Symbol = "collector",
				Title = "Collection Center",
				Description = "Resources can be desposited here so that laborers don't have to go all the way back to the Giving Tree.",
				Emissions = 0.3f,
				TakesDamage = true,
				Unlocked = true
			});

			units.Add (typeof (DrillablePlot), new UnitSettings {
				Symbol = "drillable",
				Title = "Drillable Plot",
				Description = "This plot can be drilled for milkshakes.",
				Emissions = 0f,
				TakesDamage = false
			});

			units.Add (typeof (Plot), new UnitSettings {
				Symbol = "plot",
				Title = "Plot",
				Description = "This plot can be built on.",
				Emissions = 0f,
				TakesDamage = false
			});

			units.Add (typeof (GivingTreeUnit), new UnitSettings {
				Symbol = "tree",
				Title = "Giving Tree",
				Description = "The Giving Tree gives birth to laborers and is also a portal to the next dimension.",
				Emissions = 0f,
				TakesDamage = false,
				RemovesFogOfWar = true
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

			units.Add (typeof (House), new UnitSettings {
				Symbol = "house",
				Title = "House",
				Description = "Houses increase the amount of laborers you can birth",
				Emissions = 0.1f,
				TakesDamage = true,
				Unlocked = true
			});

			units.Add (typeof (Apartment), new UnitSettings {
				Symbol = "apartment",
				Title = "Apartment",
				Description = "Apartments increase the amount of laborers you can birth",
				Emissions = 0.3f,
				TakesDamage = true,
				Unlocked = false
			});

			units.Add (typeof (Seed), new UnitSettings {
				Symbol = "seed",
				Title = "Seed",
				Description = "Seeds give birth to beautiful flowers :)",
				Emissions = 0f,
				TakesDamage = true
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
				Pair = typeof (AcceptDeliverItem<MilkshakeGroup>),
				AlwaysPairNearest = true,
				BindCapacity = 1
			});

			tasks.Add (typeof (CollectItem<CoffeeGroup>), new TaskSettings {
				Symbol = "collect_coffee",
				Duration = 1.5f,
				AutoStart = false,
				Repeat = true,
				Pair = typeof (AcceptDeliverItem<CoffeeGroup>),
				AlwaysPairNearest = true,
				BindCapacity = 1
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

			tasks.Add (typeof (CollectItem<HappinessGroup>), new TaskSettings {
				Symbol = "collect_happiness",
				Duration = 0.33f,
				AutoStart = false,
				Repeat = true
			});

			/**
			 *	Construct
			 */

			tasks.Add (typeof (ConstructUnit<CoffeePlant>), new CostTaskSettings {
				Symbol = "construct_coffee",
				Title = "Birth Coffee Plant",
				Description = "Creates a new coffee plant",
				Duration = 0f,
				Costs = new [] {
					new Dictionary<string, int> {
						{ "Milkshakes", 20 }
					}
				},
				ConstructionTargets = new [] { "plot", "drillable" }
			});

			tasks.Add (typeof (ConstructUnit<MilkshakePool>), new CostTaskSettings {
				Symbol = "construct_derrick",
				Title = "Birth Milkshake Derrick",
				Description = "Creates a new milkshake derrick",
				Duration = 0f,
				Costs = new [] {
					new Dictionary<string, int> {
						{ "Milkshakes", 20 }
					}
				},
				ConstructionTargets = new [] { "drillable" }
			});

			tasks.Add (typeof (ConstructUnit<University>), new CostTaskSettings {
				Symbol = "construct_university",
				Title = "Birth University",
				Description = "Creates a new university",
				Duration = 0f,
				Costs = new [] {
					new Dictionary<string, int> {
						{ "Milkshakes", 50 }
					}
				},
				ConstructionTargets = new [] { "plot", "drillable" }
			});

			tasks.Add (typeof (ConstructUnit<Clinic>), new CostTaskSettings {
				Symbol = "construct_clinic",
				Title = "Birth Clinic",
				Description = "Creates a new clinic",
				Duration = 0f,
				Costs = new [] {
					new Dictionary<string, int> {
						{ "Milkshakes", 40 }
					}
				},
				ConstructionTargets = new [] { "plot", "drillable" }
			});

			tasks.Add (typeof (ConstructUnit<Flower>), new CostTaskSettings {
				Symbol = "construct_flower",
				Title = "Birth Flower",
				Description = "Creates a new flower",
				Duration = 0f,
				Costs = new [] {
					new Dictionary<string, int> {
						{ "Milkshakes", 10 }
					}
				},
				ConstructionTargets = new [] { "plot", "drillable" }
			});

			tasks.Add (typeof (ConstructUnit<CollectionCenter>), new CostTaskSettings {
				Symbol = "construct_collector",
				Title = "Birth Collection Center",
				Description = "Creates a new collection center",
				Duration = 0f,
				Costs = new [] {
					new Dictionary<string, int> {
						{ "Milkshakes", 60 }
					}
				},
				ConstructionTargets = new [] { "plot", "drillable" }
			});

			tasks.Add (typeof (ConstructUnit<House>), new CostTaskSettings {
				Symbol = "construct_house",
				Title = "Birth House",
				Description = "Creates a new house so that more laborer can be birthed",
				Duration = 0f,
				Costs = new [] {
					new Dictionary<string, int> {
						{ "Milkshakes", 25 }
					}
				},
				ConstructionTargets = new [] { "plot", "drillable" }
			});

			tasks.Add (typeof (ConstructUnit<Apartment>), new CostTaskSettings {
				Symbol = "construct_apartment",
				Title = "Birth Apartment",
				Description = "Creates a new apartment so that more laborer can be birthed",
				Duration = 0f,
				Costs = new [] {
					new Dictionary<string, int> {
						{ "Milkshakes", 60 }
					}
				},
				ConstructionTargets = new [] { "plot", "drillable" }
			});

			/**
			 *	Consume
			 */

			tasks.Add (typeof (ConsumeItem<YearGroup>), new TaskSettings {
				Symbol = "consume_year",
				Title = "",
				Description = "Consumes year",
				Duration = 1f,
				AutoStart = false,
				Repeat = true,
				Pair = null
			});

			tasks.Add (typeof (ConsumeItem<HappinessGroup>), new TaskSettings {
				Symbol = "consume_happiness",
				Title = "",
				Description = "Consumes happiness",
				Duration = 3f,
				AutoStart = true,
				Repeat = true,
				Pair = null
			});

			tasks.Add (typeof (ConsumeItem<LaborGroup>), new TaskSettings {
				Symbol = "consume_labor",
				Description = "Consumes labor",
				Duration = 1f,
				Repeat = true		
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
				Symbol = "deliver_coffee",
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
				Duration = 3f,
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

			tasks.Add (typeof (GenerateLaborer), new CostTaskSettings {
				Symbol = "generate_laborer",
				Title = "Birth Laborer",
				Description = "Creates a new laborer",
				Duration = 0f,
				AutoStart = false,
				Repeat = false,
				Pair = null,
				Costs = new [] {
					new Dictionary<string, int> {
						{ "Coffee", 25 }
					}
				}
			});

			tasks.Add (typeof (GenerateUnit<Elder>), new CostTaskSettings {
				Symbol = "generate_elder",
				Title = "Birth Elder",
				Description = "Creates a new elder",
				Duration = 0f,
				AutoStart = false,
				Repeat = false,
				Pair = null,
				Costs = new [] {
					new Dictionary<string, int> {
						{ "Coffee", 0 }
					}
				}
			});

			tasks.Add (typeof (GenerateUnit<Corpse>), new CostTaskSettings {
				Symbol = "generate_corpse",
				Title = "Birth Corpse",
				Description = "Creates a new corpse",
				Duration = 0f,
				AutoStart = false,
				Repeat = false,
				Pair = null,
				Costs = new [] {
					new Dictionary<string, int> {
						{ "Coffee", 0 }
					}
				}
			});

			/**
			 *	ResearchUpgrade
			 */

			tasks.Add (typeof (ResearchUpgrade<CoffeeCapacity>), new CostTaskSettings {
				Symbol = "reseach_coffee",
				Title = "+1 coffee capacity",
				Description = "Laborers will be able to carry more coffee",
				AutoStart = false,
				Repeat = false,
				Costs = new [] {
					new Dictionary<string, int> {
						{ "Coffee", 30 }
					},
					new Dictionary<string, int> {
						{ "Coffee", 50 }
					},
					new Dictionary<string, int> {
						{ "Coffee", 150 }
					}
				}
			});

			tasks.Add (typeof (ResearchUpgrade<MilkshakeCapacity>), new CostTaskSettings {
				Symbol = "reseach_milkshake",
				Title = "+1 milkshake capacity",
				Description = "Laborers will be able to carry more milkshake",
				Costs = new [] {
					new Dictionary<string, int> {
						{ "Coffee", 50 }
					},
					new Dictionary<string, int> {
						{ "Coffee", 100 }
					},
					new Dictionary<string, int> {
						{ "Coffee", 200 }
					}
				}
			});

			/**
			 *	ResearchUnit
			 */

			tasks.Add (typeof (ResearchUnit<Apartment>), new CostTaskSettings {
				Symbol = "research_apartment",
				Title = "Apartment",
				Description = "Apartments hold more laborers and are more efficiently priced",
				Costs = new [] {
					new Dictionary<string, int> {
						{ "Coffee", 50 },
						{ "Milkshakes", 30 }
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
				Costs = new [] {
					new Dictionary<string, int> {
						{ "Milkshakes", 15 }
					}
				}
			});

			tasks.Add (typeof (UpgradeLevee), new CostTaskSettings {
				Title = "Raise levee",
				Description = "Raises the levee wall",
				Duration = 0f,
				AutoStart = false,
				Repeat = false,
				Pair = null,
				Costs = new [] {
					new Dictionary<string, int> {
						{ "Coffee", 15 },
						{ "Milkshakes", 30 }
					},
					new Dictionary<string, int> {
						{ "Coffee", 30 },
						{ "Milkshakes", 60 }
					},
					new Dictionary<string, int> {
						{ "Coffee", 45 },
						{ "Milkshakes", 90 }
					},
					new Dictionary<string, int> {
						{ "Coffee", 60 },
						{ "Milkshakes", 120 }
					},
					new Dictionary<string, int> {
						{ "Coffee", 90 },
						{ "Milkshakes", 180 }
					},
					new Dictionary<string, int> {
						{ "Coffee", 120 },
						{ "Milkshakes", 240 }
					},
					new Dictionary<string, int> {
						{ "Coffee", 180 },
						{ "Milkshakes", 320 }
					},
					new Dictionary<string, int> {
						{ "Coffee", 240 },
						{ "Milkshakes", 480 }
					}
				}
			});

			tasks.Add (typeof (UpgradeFogOfWar), new CostTaskSettings {
				Title = "Increase eyesight",
				Description = "Reveal more from fog of war",
				Duration = 0f,
				AutoStart = false,
				Repeat = false,
				Pair = null,
				Costs = new [] {
					new Dictionary<string, int> {
						{ "Coffee", 100 },
						{ "Milkshakes", 150 }
					}
				}
			});

			tasks.Add (typeof (PlantSeed), new TaskSettings {
				Title = "Plant seed",
				Description = "Seeds grow into beautiful flowers"
			});

			tasks.Add (typeof (BorrowLoan<MilkshakeLoanGroup>), new TaskSettings {
				Title = "Borrow Milkshakes",
				Description = "Take out a loan of 100 milkshakes"
			});

			tasks.Add (typeof (BorrowLoan<CoffeeLoanGroup>), new TaskSettings {
				Title = "Borrow Coffee",
				Description = "Take out a loan of 100 coffee"
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

			tasks.Add (typeof (GenerateUnitTest<Laborer>), new CostTaskSettings {
				Title = "Generate Laborer",
				Description = "Generates a laborer",
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

	public class LoansSettings {
			
		Dictionary<System.Type, LoanSettings> loans;

		public Dictionary<System.Type, LoanSettings> Loans {
			get { return loans; }
		}

		public LoanSettings this[System.Type loanType] {
			get { 
				try {
					return loans[loanType]; 
				} catch {
					throw new System.Exception ("Could not find a model for '" + loanType + "'");
				}
			}
		}

		public LoansSettings () {

			loans = new Dictionary<System.Type, LoanSettings> ();

			loans.Add (typeof (Loan<MilkshakeGroup>), new LoanSettings {
				Symbol = "milkshake_loan",
				Amount = 150,
				InterestRate = 0.06f,
				RepaymentLength = 8,
				GracePeriod = 12
			});

			loans.Add (typeof (Loan<CoffeeGroup>), new LoanSettings {
				Symbol = "coffee_loan",
				Amount = 100,
				InterestRate = 0.06f,
				RepaymentLength = 8,
				GracePeriod = 12
			});
		}
	}

	public class TaskSettings {
		public string Symbol { get; set; }
		public string Title { get; set; }			// How the task will be displayed in the UI
		public string Description { get; set; }		// A description of the task to display in the UI
		public System.Type Pair { get; set; }		// (optional) A type of AcceptorTask that this task must be bound to in order to be performed
		public float Duration { get; set; }			// How long it takes for the task to be performed
		public bool AutoStart { get; set; }			// If true, task will be performed on instantiation
		public bool Repeat { get; set; }			// If true, task will automatically repeat until it becomes disabled, if ever
		public bool AlwaysPairNearest { get; set; } // If false, task will always return to the first pair it found
		public int BindCapacity { get; set; }		// (optional) The maximum number of tasks that the Pair can bind with (ignored if Pair is null)
	}

	public class CostTaskSettings : TaskSettings {

		// An array of costs. The CostTask will use the Costs at the first array position, then iterate the array if there are more elements.
		// This is useful for e.g. upgrades
		// <ItemGroup ID, amount required>
		public Dictionary<string, int>[] Costs { get; set; }
		public string[] ConstructionTargets { get; set; }
	}

	public class UnitSettings {
		public string Symbol { get; set; }
		public string Title { get; set; }			// Display name
		public string Description { get; set; }		// A description to display in the ui
		public float Emissions { get; set; }		// (%) environmental impact of the unit (0 = no impact, 1 = most impact)
		public bool TakesDamage { get; set; }		// Whether or not the unit is damaged by water
		public bool RemovesFogOfWar { get; set; }	// True if the unit, when built, removes the fog of war surrounding it
		public bool Unlocked { get; set; }			// True if the unit has been unlocked through research
	}

	public class LoanSettings {
		public string Symbol { get; set; } // not currently doing anything
		public string Resource { get; set; }
		public int Amount { get; set; }
		public float InterestRate { get; set; }
		public int RepaymentLength { get; set; }
		public int GracePeriod { get; set; }
	}
}
