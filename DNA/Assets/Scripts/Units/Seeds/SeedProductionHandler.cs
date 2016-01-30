using UnityEngine;
using System.Collections;

namespace DNA.Units {

	public class SeedProductionHandler {

		float duration = 2f;
		Transform producerTransform;
		float offset;
		Seed seed;
		Co coSeed;

		public SeedProductionHandler (Transform producerTransform, float offset) {
			this.producerTransform = producerTransform;
			this.offset = offset;
			coSeed = Co.Start (duration, ProduceSeed, OnProduceSeed);
		}

		public void Stop () {
			coSeed.Stop (false);
		}

		void ProduceSeed (float t) {}

		void OnProduceSeed () {
			if (seed == null) {
				seed = ObjectPool.Instantiate<Seed> (
					new Vector3 (producerTransform.position.x, producerTransform.position.y + offset, producerTransform.position.z));
			}
			coSeed = Co.Start (duration, ProduceSeed, OnProduceSeed);
		}
	}
}