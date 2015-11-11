#define DEBUG
using UnityEngine;
using System.Collections;

namespace DNA {

	public class OuterSea : Sea2 {

		public Levee levee;
		float riseCoefficient = 
		#if DEBUG
		0.001f
		#else
		0.00001f
		#endif
		;

		protected override void Awake () {
			base.Awake ();
			foreach (Transform child in MyTransform) {
				child.GetComponent<Renderer> ().SetColor (Palette.Blue);
			}
		}

		void OnEnable () {
			EmissionsManager.onUpdate += OnUpdateEmissions;
		}

		void OnDisable () {
			EmissionsManager.onUpdate -= OnUpdateEmissions;
		}

		void OnUpdateEmissions (float val) {
			riseRate = val;
		}

		void Update () {
			Level += riseRate * riseCoefficient;
			#if DEBUG
			if (Input.GetKeyDown (KeyCode.Z)) {
				riseRate += 1f;
			}
			if (Input.GetKeyDown (KeyCode.X)) {
				riseRate -= 1f;
			}
			#endif
		}
	}
}