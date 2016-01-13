using UnityEngine;
using System.Collections;

namespace DNA {

	public class SeaManager : MBRefs {

		public float SeaLevel {
			get { return inner.Level; }
		}

		public Levee levee;
		public OuterSea outer;
		public InnerSea inner;

		float pumpRate = 0f;
		float riseRate = 0f;
		float displacementCoefficient = 0.000033f;

		public float LeveeTop {
			get { return levee.Position.y + levee.Height; }
		}

		void OnEnable () { EmissionsManager.onUpdate += OnUpdateEmissions; }
		void OnDisable () { EmissionsManager.onUpdate -= OnUpdateEmissions; }

		[DebuggableMethod ()]
		void OnUpdateEmissions (float val) {
			riseRate = val * displacementCoefficient;
		}

		[DebuggableMethod ()]
		void SetPumpRate (float rate) {
			pumpRate = rate * displacementCoefficient;
		}

		[DebuggableMethod ()]
		void SimulateFlood () {
			outer.Level = outer.MaxLevel;
			inner.Level = inner.MaxLevel;
			Coroutine.WaitForSeconds (1f, () => {
				outer.Level = outer.MinLevel;
				inner.Level = inner.MinLevel;
			});
		}

		void Update () {
			
			// Outer sea rises until it reaches top of the levee
			if (outer.Level <= LeveeTop) {
				outer.Level += riseRate;

				// Inner sea is pumped if levee hasn't been breached
				inner.Level -= pumpRate;
			}

			// Inner sea rises if outer sea has reached top of levee
			if (outer.Level >= LeveeTop) {
				inner.Level += riseRate;
			}

			// Both seas rise once inner sea reaches top of levee
			if (inner.Level >= LeveeTop) {
				outer.Level = inner.Level;
			}
		}
	}
}