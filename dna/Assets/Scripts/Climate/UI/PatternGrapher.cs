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

		TextMesh text = null;
		TextMesh Text {
			get {
				if (text == null) {
					text = GetComponent<TextMesh> ();
				}
				return text;
			}
		}

		public void SetPattern (Pattern pattern, string patternName="") {
			this.pattern = pattern;
			Text.text = patternName;
		}

		public void SetScreenPosition (float yPos) {
			transform.position = Camera.main.ViewportToWorldPoint (new Vector3 (0, yPos, 10f));
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
				points[i].startColor = Color.black;
				points[i].startSize = 0.1f;
			}

			Particles.SetParticles(points, points.Length);
		}
	}
}