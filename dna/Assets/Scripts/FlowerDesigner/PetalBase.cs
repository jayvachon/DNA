using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace DNA.FlowerDesigner {

	public class PetalBase : Part<Petal> {

		[Range (1, 10)]
		public int _petalLength = 1;

		[Range (0.1f, 10)]
		public float _petalScale = 1f;

		[Range (0, 1.58f)]
		public float _petalScaleCurvePhase = 0.5f;

		protected override void OnUpdatePartCount (List<Petal> petals) {
			UpdatePetalPositions (petals);
			UpdatePetalSettings (petals);
		}

		public void UpdatePetalPositions (List<Petal> petals) {

			float interval = 360f / (float)petals.Count;

			for (int i = 0; i < petals.Count; i ++) {
				petals[i].MyTransform.SetLocalEulerAnglesY ((float)i * interval);
			}
		}

		public void UpdatePetalSettings (List<Petal> petals) {
			foreach (Petal petal in petals) {
				petal._partCount = _petalLength;
				petal._scale = _petalScale;
				petal._scaleCurvePhase = _petalScaleCurvePhase;
			}
		}
	}
}