using UnityEngine;
using System.Collections;

public class FogController : MonoBehaviour {

	Color grey = new Color (0.5f, 0.5f, 0.5f);
	Color fogLight = new Color (0.224f, 0.286f, 0);
	Color fogHeavy = new Color (0.157f, 0.055f, 0.329f);

	[DebuggableMethod ()]
	public void UpdateRain (float intensity) {
		RenderSettings.fogDensity = Mathf.Lerp (0.007f, 0.02f, intensity);
		RenderSettings.fogColor = Color.Lerp (fogLight, fogHeavy, intensity);
		RenderSettings.ambientLight = Color.Lerp (Color.white, grey, intensity);
	}
}
