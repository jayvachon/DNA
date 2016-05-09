using UnityEngine;
using System.Collections;

namespace DNA.Climate {

	[System.Serializable]
	public class NoisySignal : Pattern {

		public Noise noise;
		public Wave wave;

		public NoisySignal (Noise noise, Wave wave) {
			this.noise = noise;
			this.wave = wave;
		}

		public override void Update () {
			base.Update ();
			noise.Cursor = Cursor;
			wave.Cursor = Cursor;
		}

		public override float ValueAt (float position) {
			return (noise.ValueAt (position) + wave.ValueAt (position)) / (noise.amplitude + wave.amplitude);
		}
	}
}