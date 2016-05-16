using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using DNA.Tasks;

namespace DNA.Units {

	public class Seed : Unit {

		SeedProductionHandler seedProduction;

		public void Init (SeedProductionHandler seedProduction) {
			this.seedProduction = seedProduction;
		}

		protected override void OnInitPerformableTasks (PerformableTasks p) {
			p.Add (new PlantSeed ()).onStart += OnStartPlant;
		}

		public override void OnPointerDown (PointerEventData e) {
			TaskPen.Set (PerformableTasks[typeof (PlantSeed)]);
			TaskPen.onRemove += OnCancelPlant;
			unitRenderer.Hide ();
		}

		void OnCancelPlant () {
			TaskPen.onRemove -= OnCancelPlant;
			unitRenderer.Show ();
		}

		void OnStartPlant (PerformerTask task) {
			TaskPen.onRemove -= OnCancelPlant;
			seedProduction.RemoveSeed ();
		}
	}
}