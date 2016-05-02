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
		float floodRate = 0.0033f;
		float displacementCoefficient = 0.00003f;

		public float LeveeTop {
			get { return levee.Position.y + levee.Height; }
		}

		void OnEnable () { EmissionsManager.onUpdate += OnUpdateEmissions; }
		void OnDisable () { EmissionsManager.onUpdate -= OnUpdateEmissions; }

		[DebuggableMethod ()]
		void OnUpdateEmissions (float val) {
			riseRate = val * displacementCoefficient;
			outer.RiseRate = riseRate;
		}

		[DebuggableMethod ()]
		void SetPumpRate (float rate) {
			pumpRate = rate * displacementCoefficient;
		}

		/*[DebuggableMethod ()]
		void SimulateFlood () {
			outer.Level = outer.MaxLevel;
			inner.Level = inner.MaxLevel;
			Co2.WaitForSeconds (1f, () => {
				outer.Level = outer.MinLevel;
				inner.Level = inner.MinLevel;
			});
		}*/

		[DebuggableMethod ()]
		void UpgradeLevee () {
			Upgrades.Instance.NextLevel<LeveeHeight> ();
		}

		void Update () {
			
			// Outer sea rises until it reaches top of the levee
			if (outer.Level <= LeveeTop) {
				// outer.Level += riseRate;

				// Inner sea is pumped if levee hasn't been breached
				// inner.Level -= pumpRate;
				inner.RiseRate = -pumpRate;
			}

			// Inner sea rises if outer sea has reached top of levee
			if (outer.Level >= LeveeTop && inner.Level < LeveeTop) {
				// inner.Level += floodRate;
				inner.RiseRate = floodRate;
			}

			// Both seas rise once inner sea reaches top of levee
			if (inner.Level >= LeveeTop) {
				// TODO: set inner sea level = to outer sea level
				inner.RiseRate = 0f;
				inner.SetLevel (outer.Level);
				// inner.Level += riseRate;
				// outer.Level = inner.Level;
			}
		}
	}
}