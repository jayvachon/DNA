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
				Description = "Deliver remains to the Giving Tree to harvest the years."
			});

			units.Add (typeof (Shark), new UnitSettings {
				Symbol = "shark",
				Title = "Shark",
				Description = "Sharks are here to collect loans"
			});

			units.Add (typeof (MilkshakePool), new UnitSettings {
				Symbol = "derrick",
				Title = "Milkshake Derrick",
				Description = "Milkshakes collected from a Derrick can be used to construct buildings.",
				Emissions = 1f,
				TakesDamage = true,
				Unlocked = true,
				RemovesFogOfWar = true,
				Demolishable = true
			});

			units.Add (typeof (Turret), new UnitSettings {
				Symbol = "turret",
				Title = "Turret",
				Description = "Used to defend against sharks",
				Emissions = 0.5f,
				TakesDamage = true,
				Unlocked = true,
				RemovesFogOfWar = true,
				Demolishable = true
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
				Unlocked = true,
				RemovesFogOfWar = true,
				Demolishable = true
			});

			units.Add (typeof (Clinic), new UnitSettings {
				Symbol = "clinic",
				Title = "Clinic",
				Description = "Elders live longer when they're receiving care at a Clinic.",
				Emissions = 0.75f,
				TakesDamage = true,
				Unlocked = false,
				RemovesFogOfWar = true,
				Demolishable = true
			});

			units.Add (typeof (Flower), new UnitSettings {
				Symbol = "flower",
				Title = "Flower",
				Description = "Flowers are really pretty :)",
				Emissions = -0.05f,
				TakesDamage = true,
				RemovesFogOfWar = true,
				Demolishable = true
			});

			units.Add (typeof (CollectionCenter), new UnitSettings {
				Symbol = "collector",
				Title = "Silo",
				Description = "Resources can be desposited here so that laborers don't have to go all the way back to the Giving Tree.",
				Emissions = 0.3f,
				TakesDamage = true,
				Unlocked = false,
				Demolishable = true
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
				Unlocked = true,
				RemovesFogOfWar = true,
				Demolishable = true
			});

			units.Add (typeof (Apartment), new UnitSettings {
				Symbol = "apartment",
				Title = "Apartment",
				Description = "Apartments increase the amount of laborers you can birth",
				Emissions = 0.3f,
				TakesDamage = true,
				Unlocked = false,
				RemovesFogOfWar = true,
				Demolishable = true
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

		Dictionary<string, TaskSettings> tasks;

		public Dictionary<string, TaskSettings> Tasks {
			get { return tasks; }
		}

		public TaskSettings this[string symbol] {
			get {
				try {
					return tasks[symbol];
				} catch {
					throw new System.Exception ("Could not find a model for '" + symbol + "'");
				}
			}
		}

		public TasksSettings () {
			
			tasks = new Dictionary<string, TaskSettings> ();
			
			/**
			 *	CollectItem
			 */

			tasks.Add ("collect_milkshake", new TaskSettings {
				Type = typeof (CollectItem<MilkshakeGroup>),
				Duration = 1f,
				Repeat = true,
				Pair = typeof (AcceptDeliverItem<MilkshakeGroup>),
				AlwaysPairNearest = true,
				BindCapacity = 1
			});

			tasks.Add ("shark_collect_milkshake", new TaskSettings {
				Type = typeof (CollectItem<MilkshakeGroup>),
				Duration = 0.2f,
				Repeat = true,
				Pair = typeof (AcceptDeliverItem<MilkshakeGroup>)
			});

			tasks.Add ("collect_coffee", new TaskSettings {
				Type = typeof (CollectItem<CoffeeGroup>),
				Duration = 0.75f,
				Repeat = true,
				Pair = typeof (AcceptDeliverItem<CoffeeGroup>),
				AlwaysPairNearest = true,
				BindCapacity = 1
			});

			tasks.Add ("shark_collect_coffee", new TaskSettings {
				Type = typeof (CollectItem<CoffeeGroup>),
				Duration = 0.2f,
				Repeat = true,
				Pair = typeof (AcceptDeliverItem<CoffeeGroup>)
			});

			tasks.Add ("collect_labor", new TaskSettings {
				Type = typeof (CollectItem<LaborGroup>),
				Duration = 0.5f,
				Repeat = true
			});

			tasks.Add ("collect_health", new TaskSettings {
				Type = typeof (CollectItem<HealthGroup>),
				Duration = 1f,
				Repeat = true
			});

			tasks.Add ("collect_happiness", new TaskSettings {
				Type = typeof (CollectItem<HappinessGroup>),
				Duration = 0.165f,
				Repeat = true
			});

			/**
			 *	Construct
			 */

			tasks.Add ("cancel_construction", new TaskSettings {
				Type = typeof (CancelConstruction),
				Title = "Cancel construction"
			});

			tasks.Add ("demolish_unit", new TaskSettings {
				Type = typeof (DemolishUnit),
				Title = "Demolish"
			});

			tasks.Add ("construct_coffee", new CostTaskSettings {
				Type = typeof (ConstructUnit<CoffeePlant>),
				Title = "Birth Coffee Plant",
				Description = "Creates a new coffee plant",
				Costs = new [] {
					new Dictionary<string, int> {
						{ "Milkshakes", 20 }
					}
				},
				ConstructionTargets = new [] { "plot", "drillable" }
			});

			tasks.Add ("construct_derrick", new CostTaskSettings {
				Type = typeof (ConstructUnit<MilkshakePool>),
				Title = "Birth Milkshake Derrick",
				Description = "Creates a new milkshake derrick",
				Costs = new [] {
					new Dictionary<string, int> {
						{ "Milkshakes", 20 }
					}
				},
				ConstructionTargets = new [] { "drillable" }
			});

			tasks.Add ("construct_university", new CostTaskSettings {
				Type = typeof (ConstructUnit<University>),
				Title = "Birth University",
				Description = "Creates a new university",
				Costs = new [] {
					new Dictionary<string, int> {
						{ "Milkshakes", 50 }
					}
				},
				ConstructionTargets = new [] { "plot", "drillable" }
			});

			tasks.Add ("construct_clinic", new CostTaskSettings {
				Type = typeof (ConstructUnit<Clinic>),
				Title = "Birth Clinic",
				Description = "Creates a new clinic",
				Costs = new [] {
					new Dictionary<string, int> {
						{ "Milkshakes", 40 }
					}
				},
				ConstructionTargets = new [] { "plot", "drillable" }
			});

			tasks.Add ("construct_flower", new CostTaskSettings {
				Type = typeof (ConstructUnit<Flower>),
				Title = "Birth Flower",
				Description = "Creates a new flower",
				Costs = new [] {
					new Dictionary<string, int> {
						{ "Milkshakes", 10 }
					}
				},
				ConstructionTargets = new [] { "plot", "drillable" }
			});

			tasks.Add ("construct_collector", new CostTaskSettings {
				Type = typeof (ConstructUnit<CollectionCenter>),
				Title = "Birth Silo",
				Description = "Creates a new silo",
				Costs = new [] {
					new Dictionary<string, int> {
						{ "Milkshakes", 60 }
					}
				},
				ConstructionTargets = new [] { "plot", "drillable" }
			});

			tasks.Add ("construct_house", new CostTaskSettings {
				Type = typeof (ConstructUnit<House>),
				Title = "Birth House",
				Description = "Creates a new house so that more laborer can be birthed",
				Costs = new [] {
					new Dictionary<string, int> {
						{ "Milkshakes", 25 }
					}
				},
				ConstructionTargets = new [] { "plot", "drillable" }
			});

			tasks.Add ("construct_apartment", new CostTaskSettings {
				Type = typeof (ConstructUnit<Apartment>),
				Title = "Birth Apartment",
				Description = "Creates a new apartment so that more laborer can be birthed",
				Costs = new [] {
					new Dictionary<string, int> {
						{ "Milkshakes", 60 }
					}
				},
				ConstructionTargets = new [] { "plot", "drillable" }
			});

			tasks.Add ("construct_turret", new CostTaskSettings {
				Type = typeof (ConstructUnit<Turret>),
				Title = "Birth Turret",
				Description = "Turrets kill loan sharks >:)",
				Costs = new [] {
					new Dictionary<string, int> {
						{ "Milkshakes", 40 }
					}
				},
				ConstructionTargets = new [] { "plot", "drillable" }
			});

			/**
			 *	Consume
			 */

			tasks.Add ("consume_year", new TaskSettings {
				Type = typeof (ConsumeItem<YearGroup>),
				Description = "Consume year",
				Duration = 1f,
				AutoStart = false,
				Repeat = true,
				Pair = null
			});

			tasks.Add ("consume_happiness", new TaskSettings {
				Type = typeof (ConsumeItem<HappinessGroup>),
				Duration = 2f,
				AutoStart = true,
				Repeat = true,
				Pair = null
			});

			tasks.Add ("consume_labor", new TaskSettings {
				Type = typeof (ConsumeItem<LaborGroup>),
				Duration = 0.5f,
				Repeat = true		
			});

			tasks.Add ("workplace_consume_labor", new TaskSettings {
				Type = typeof (WorkplaceConsumeItem<LaborGroup>),
				Duration = 0.5f,
				Repeat = true		
			});

			tasks.Add ("consume_health", new TaskSettings {
				Type = typeof (ConsumeItem<HealthGroup>),
				Duration = 0.05f,
				Repeat = true
			});

			/**
			 *	DeliverItem
			 */

			tasks.Add ("deliver_milkshake", new TaskSettings {
				Type = typeof (DeliverItem<MilkshakeGroup>),
				Description = "Delivers milkshakes",
				Duration = 1f,
				AutoStart = false,
				Repeat = true,
				Pair = typeof (AcceptCollectItem<MilkshakeGroup>)
			});

			tasks.Add ("deliver_coffee", new TaskSettings {
				Type = typeof (DeliverItem<CoffeeGroup>),
				Description = "Delivers coffee",
				Duration = 0.75f,
				AutoStart = false,
				Repeat = true,
				Pair = typeof (AcceptCollectItem<CoffeeGroup>)
			});

			tasks.Add ("workplace_deliver_milkshake", new TaskSettings {
				Type = typeof (WorkplaceDeliverItem<MilkshakeGroup>),
				Duration = 2.5f,
				Repeat = true
			});

			tasks.Add ("workplace_deliver_coffee", new TaskSettings {
				Type = typeof (WorkplaceDeliverItem<CoffeeGroup>),
				Duration = 3f,
				Repeat = true
			});

			/**
			 *	GenerateItem
			 */

			tasks.Add ("generate_coffee", new TaskSettings {
				Type = typeof (GenerateItem<CoffeeGroup>),
				Duration = 1.5f,
				AutoStart = true,
				Repeat = true,
				Pair = null
			});

			tasks.Add ("generate_year", new TaskSettings {
				Type = typeof (GenerateItem<YearGroup>),
				Duration = 1f,
				AutoStart = true,
				Repeat = true,
				Pair = null
			});

			tasks.Add ("generate_health", new TaskSettings {
				Type = typeof (GenerateItem<HealthGroup>),
				Duration = 3f,
				AutoStart = true,
				Repeat = true,
				Pair = null
			});

			/**
			 *	GenerateUnit
			 */

			tasks.Add ("generate_laborer", new CostTaskSettings {
				Type = typeof (GenerateLaborer),
				Title = "Birth Laborer",
				Description = "Creates a new laborer",
				Costs = new [] {
					new Dictionary<string, int> {
						{ "Coffee", 25 }
					}
				}
			});

			tasks.Add ("generate_elder", new CostTaskSettings {
				Type = typeof (GenerateUnit<Elder>),
				Title = "Birth Elder",
				Description = "Creates a new elder",
				Costs = new [] {
					new Dictionary<string, int> {
						{ "Coffee", 0 }
					}
				}
			});

			tasks.Add ("generate_corpse", new CostTaskSettings {
				Type = typeof (GenerateUnit<Corpse>),
				Title = "Birth Corpse",
				Description = "Creates a new corpse",
				Costs = new [] {
					new Dictionary<string, int> {
						{ "Coffee", 0 }
					}
				}
			});

			/**
			 *	ResearchUpgrade
			 */

			tasks.Add ("reseach_laborer_speed", new CostTaskSettings {
				Type = typeof (ResearchUpgrade<LaborerSpeed>),
				Title = "Faster Workers",
				Description = "Laborers will move faster",
				Costs = new [] {
					new Dictionary<string, int> {
						{ "Coffee", 60 }
					},
					new Dictionary<string, int> {
						{ "Coffee", 120 }
					},
					new Dictionary<string, int> {
						{ "Coffee", 200 }
					},
					new Dictionary<string, int> {
						{ "Coffee", 320 }
					},
					new Dictionary<string, int> {
						{ "Coffee", 480 }
					},
					new Dictionary<string, int> {
						{ "Coffee", 720 }
					}
				}
			});

			tasks.Add ("reseach_coffee", new CostTaskSettings {
				Type = typeof (ResearchUpgrade<CoffeeCapacity>),
				Title = "+1 coffee capacity",
				Description = "Laborers will be able to carry more coffee",
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

			tasks.Add ("reseach_milkshake", new CostTaskSettings {
				Type = typeof (ResearchUpgrade<MilkshakeCapacity>),
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

			tasks.Add ("research_apartment", new CostTaskSettings {
				Type = typeof (ResearchUnit<Apartment>),
				Title = "Apartment",
				Description = "Apartments hold more laborers and are more efficiently priced",
				Costs = new [] {
					new Dictionary<string, int> {
						{ "Coffee", 50 },
						{ "Milkshakes", 30 }
					}
				}
			});

			tasks.Add ("research_collection", new CostTaskSettings {
				Type = typeof (ResearchUnit<CollectionCenter>),
				Title = "Silo",
				Description = "Workers can deliver resources to the silo instead of going to the Giving Tree",
				Costs = new [] {
					new Dictionary<string, int> {
						{ "Coffee", 60 },
						{ "Milkshakes", 90 }
					}
				}
			});

			/**
			 *	Misc
			 */

			tasks.Add ("flee_tree", new CostTaskSettings {
				Title = "Flee Tree",
				Type = typeof (FleeTree),
				Description = "Goes to the next level",
				Costs = new [] {
					new Dictionary<string, int> {
						{ "Milkshakes", 1 }
					}
				}
			});

			tasks.Add ("construct_road", new CostTaskSettings {
				Title = "Birth road",
				Type = typeof (ConstructRoad),
				Description = "Builds a road",
				Costs = new [] {
					new Dictionary<string, int> {
						{ "Milkshakes", 10 }
					}
				}
			});

			tasks.Add ("upgrade_levee", new CostTaskSettings {
				Title = "Raise levee",
				Type = typeof (UpgradeLevee),
				Description = "Raises the levee wall",
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

			tasks.Add ("research_eyesight", new CostTaskSettings {
				Title = "Increase eyesight",
				Type = typeof (ResearchUpgrade<Eyesight>),
				Description = "Reveal more from fog of war",
				Costs = new [] {
					new Dictionary<string, int> {
						{ "Coffee", 100 },
						{ "Milkshakes", 150 }
					},
					new Dictionary<string, int> {
						{ "Coffee", 200 },
						{ "Milkshakes", 350 }
					}
				}
			});

			tasks.Add ("plant_seed", new TaskSettings {
				Title = "Plant seed",
				Type = typeof (PlantSeed),
				Description = "Seeds grow into beautiful flowers"
			});

			tasks.Add ("borrow_milkshakes", new TaskSettings {
				Title = "Borrow Milkshakes",
				Type = typeof (BorrowLoan<MilkshakeLoanGroup>),
				Description = "Take out a loan of 100 milkshakes"
			});

			tasks.Add ("borrow_coffee", new TaskSettings {
				Title = "Borrow Coffee",
				Type = typeof (BorrowLoan<CoffeeLoanGroup>),
				Description = "Take out a loan of 100 coffee"
			});

			/**
			 *	Tests
			 */

			/*tasks.Add (typeof (AutoStartTest), new TaskSettings {
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
			});*/
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
				InterestRate = 0.02f,
				RepaymentLength = 10,
				GracePeriod = 12
			});

			loans.Add (typeof (Loan<CoffeeGroup>), new LoanSettings {
				Symbol = "coffee_loan",
				Amount = 100,
				InterestRate = 0.02f,
				RepaymentLength = 10,
				GracePeriod = 0
				/*InterestRate = 0.06f,
				RepaymentLength = 8,
				GracePeriod = 12*/
			});
		}
	}

	public class TaskSettings {
		public System.Type Type { get; set; }
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
		public Dictionary<string, int>[] Costs { get; set; }	// Costs of task
		public string[] ConstructionTargets { get; set; }		// Types of units to construct on (ignore if this is not a construction task)
		public System.Type BuildType { get; set; }				// Type of unit to construct (ignore if this is not a construction task)
	}

	public class UnitSettings {
		public string Symbol { get; set; }
		public string Title { get; set; }			// Display name
		public string Description { get; set; }		// A description to display in the ui
		public float Emissions { get; set; }		// (%) environmental impact of the unit (0 = no impact, 1 = most impact)
		public bool TakesDamage { get; set; }		// Whether or not the unit is damaged by water
		public bool RemovesFogOfWar { get; set; }	// True if the unit, when built, removes the fog of war surrounding it
		public bool Unlocked { get; set; }			// True if the unit has been unlocked through research
		public bool Demolishable { get; set; }		// True if the unit can be demolished
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
