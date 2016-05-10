using UnityEngine;
using System.Collections;

namespace DNA.Climate {

	[System.Serializable]
	public class DelayedPattern : Pattern {

		public override float Amplitude {
			get { return amplitude; }
		}

		[Range (0, 1)]
		public float amplitude;

		Pattern source;
		float delayAmount;

		public DelayedPattern (Pattern source, float delayAmount) {
			this.source = source;
			this.delayAmount = delayAmount;
		}

		public override float ValueAt (float position) {
			return source.ValueAt (Mathf.Max (0, position - delayAmount));
		}
	}
}