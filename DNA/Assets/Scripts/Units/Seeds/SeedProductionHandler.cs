using UnityEngine;
using System.Collections;

namespace DNA.Units {

	public class SeedProductionHandler {

		float[] durations = new [] { 30f, 60f, 120f };
		int duration = 0;

		Transform producerTransform;
		float offset;
		Seed seed;
		Co coSeed;

		public SeedProductionHandler (Transform producerTransform, float offset) {
			this.producerTransform = producerTransform;
			this.offset = offset;
			coSeed = Co.Start (durations[duration], ProduceSeed, OnProduceSeed);
		}

		public void Stop () {
			coSeed.Stop (false);
		}

		public void RemoveSeed () {
			ObjectPool.Destroy<Seed> (seed);
			seed = null;
			duration += 1;
			if (duration <= durations.Length-1)
				coSeed = Co.Start (durations[duration], ProduceSeed, OnProduceSeed);
		}

		void ProduceSeed (float t) {}

		void OnProduceSeed () {
			if (seed == null) {
				seed = ObjectPool.Instantiate<Seed> (
					new Vector3 (producerTransform.position.x, producerTransform.position.y + offset, producerTransform.position.z));
				seed.Init (this);
			}
		}
	}
}