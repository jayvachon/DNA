using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace DNA.FlowerDesigner {

	public class Petal : Part<Cube> {

		// [Range (0.1f, 10)]
		public float _scale = 1f;

		[Range (0, 1.58f)]
		public float _scaleCurvePhase = 0.5f;

		protected override void OnUpdatePartCount (List<Cube> cubes) {
			UpdateScale (cubes);
			UpdatePositions(cubes);
		}

		public void UpdateScale (List<Cube> cubes) {

			for (int i = 0; i < cubes.Count; i ++) {
				cubes[i].Scale = Mathf.Sin (Mathf.PI / 2 * (float)i / (float)cubes.Count + _scaleCurvePhase);
				cubes[i].Scale = Mathf.Pow (0.1f + cubes[i].Scale, _scale);
			}
		}

		public void UpdatePositions (List<Cube> cubes) {

			if (cubes.Count == 0)
				return;

			cubes[0].LocalPosition = Vector3.forward;

			for (int i = 1; i < cubes.Count; i ++) {
				Cube c = cubes[i];
				Cube parent = cubes[i-1];
				c.LocalPosition = parent.LocalPosition + new Vector3 (0, 0, parent.Scale*0.5f + c.Scale*0.5f);
			}
		}
	}
}