using UnityEngine;
using System.Collections;

namespace DNA.Tasks {

	public class TaskDisplaySettings {

		readonly string Title;
		readonly string Description;

		public TaskDisplaySettings (string title, string description) {
			Title = title;
			Description = description;
		}
	}
}