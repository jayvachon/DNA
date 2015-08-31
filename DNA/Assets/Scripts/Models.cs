using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GameInventory;
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
			get { return tasks[taskType]; }
		}

		public TasksSettings () {
			tasks = new Dictionary<System.Type, TaskSettings> ();
			
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
}
