using UnityEngine;
using System.Collections;

namespace DNA.Climate {

	[System.Serializable]
	public class Wave : Pattern {

		public float frequency;
		
		[Range (0, 1)]
		public float amplitude;

		public override float Amplitude {
			get { return amplitude; }
		}

		float twopi;

		public Wave (float frequency=0f, float amplitude=1f) {
			twopi = Mathf.PI * 2f;
			this.frequency = frequency;
			this.amplitude = amplitude;
		}

		public override float ValueAt (float position) {
			return Mathf.Sin (twopi * position/frequency) * amplitude;
		}
	}
}