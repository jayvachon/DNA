using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using DNA.Tasks;

namespace DNA.Units {

	public class Seed : Unit, ITaskPerformer {

		#region ITaskPerformer implementation
		PerformableTasks performableTasks;
		public PerformableTasks PerformableTasks {
			get {
				if (performableTasks == null) {
					performableTasks = new PerformableTasks (this);
					performableTasks.Add (new PlantSeed ());
				}
				return performableTasks;
			}
		}
		#endregion

		public override void OnPointerDown (PointerEventData e) {
			TaskPen.Set (PerformableTasks[typeof (PlantSeed)]);
			TaskPen.onRemove += OnCancelPlant;
			unitRenderer.Hide ();
		}

		void OnCancelPlant () {
			TaskPen.onRemove -= OnCancelPlant;
			unitRenderer.Show ();
		}
	}
}