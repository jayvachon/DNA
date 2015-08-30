using UnityEngine;
using System.Collections;

namespace GameClock {

	public class Clock : MonoBehaviour {

		public class Settings {
			
			public float scale = 1;
			public float beatDuration = 1;

			public Settings (float scale=1, float beatDuration=1) {
				this.scale = scale;
				this.beatDuration = beatDuration;
			}
		}

		public class Timing {
			public float time;
			public int beatCount;
		}

		public static Clock instance = null;
		Settings settings;
		Timing timing;

		void Awake () {
			
			if (instance == null)
				instance = this;

			settings = new Settings ();
			timing = new Timing ();
			RegularTick ();
			Beat ();
		}

		void RegularTick () {
			StartCoroutine (CoRegularTick ());
		}

		IEnumerator CoRegularTick () {
			while (Application.isPlaying) {
				timing.time += Time.deltaTime * settings.scale;
				yield return null;
			}
		}

		public void Beat () {
			StartCoroutine (CoBeat ());
		}

		IEnumerator CoBeat () {

			float time = timing.time;
			float endTime = time + settings.beatDuration;

			while (time < endTime) {
				time = timing.time;
				yield return null;
			}

			timing.beatCount ++;
			Beat ();
		}

		public void WaitForBeats (ITimeable timeable, int count) {
			StartCoroutine (CoWaitForBeats (timeable, count));
		}

		IEnumerator CoWaitForBeats (ITimeable timeable, int count) {

			int beats = timing.beatCount;
			int endBeats = beats + count;

			while (beats < endBeats) {
				beats = timing.beatCount;
				yield return null;
			}

			timeable.OnBeatsElapsed ();
		}
	}
}