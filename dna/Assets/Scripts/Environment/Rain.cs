using UnityEngine;
using System.Collections;

public class Rain : MonoBehaviour {

	ParticleSystem particles = null;
	ParticleSystem Particles {
		get {
			if (particles == null) {
				particles = GetComponent<ParticleSystem> ();
			}
			return particles;
		}
	}

	[DebuggableMethod ()]
	public void UpdateRain (float intensity) {
		var em = Particles.emission;
        em.rate = ParticleValue (0, 4000, intensity);
	}

	[DebuggableMethod ()]
	public void UpdateWind (float intensity, float direction) {
		Particles.startSpeed = Mathf.Lerp (40, 80, intensity);
		var force = Particles.forceOverLifetime;
		force.x = ParticleValue (Mathf.Cos (direction * Mathf.PI * 2) * intensity * 200);
		force.y = ParticleValue (Mathf.Sin (direction * Mathf.PI * 2) * intensity * 200);
	}

	ParticleSystem.MinMaxCurve ParticleValue (float min, float max, float x) {
		return new ParticleSystem.MinMaxCurve (Mathf.Lerp (min, max, x));
	}

	ParticleSystem.MinMaxCurve ParticleValue (float val) {
		return new ParticleSystem.MinMaxCurve (val);
	}
}
