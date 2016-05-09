using UnityEngine;
using System.Collections;

namespace DNA.Climate {

	public class PatternGrapher : MonoBehaviour {

		Pattern pattern;

		ParticleSystem.Particle[] points;

		ParticleSystem particles = null;
		ParticleSystem Particles {
			get {
				if (particles == null) {
					particles = GetComponent<ParticleSystem> ();
				}
				return particles;
			}
		}

		public void SetPattern (Pattern pattern) {
			this.pattern = pattern;
		}

		void Update () {

			if (pattern == null)
				return;

			float resolution = 0.2f;
			float[] vals = pattern.LookAheadValues (240f, resolution);
			int pointCount = vals.Length;
			points = new ParticleSystem.Particle[pointCount];
			float increment = resolution * 0.05f;

			for (int i = 0; i < pointCount; i ++) {
				float x = i * increment;
				points[i].position = new Vector3 (x, vals[i], 0f);
				points[i].color = Color.black;
				points[i].size = 0.1f;
			}

			Particles.SetParticles(points, points.Length);
		}
	}
}