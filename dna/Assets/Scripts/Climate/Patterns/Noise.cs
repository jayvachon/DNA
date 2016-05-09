using UnityEngine;
using System.Collections;

namespace DNA.Climate {

	[System.Serializable]
	public class Noise : Pattern {

		public float frequency;
		
		[Range (0, 1)]
		public float amplitude;

		float seed;

		public Noise (float frequency=0f, float amplitude=1f) {
			this.frequency = frequency;
			this.amplitude = amplitude;
			seed = Random.Range (0f, 10000f);
		}

		public override float ValueAt (float position) {
			return Mathf.PerlinNoise (position/frequency, seed) * amplitude;
		}

		public static Noise None () {
			return new Noise (0f, 0f);
		}
	}
}