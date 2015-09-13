using UnityEngine;
using System.Collections;

namespace DNA.Tasks {

	public class TaskDisplaySettings {

		public readonly string Title;
		public readonly string Description;

		public TaskDisplaySettings (string title, string description) {
			Title = title;
			Description = description;
		}
	}
}