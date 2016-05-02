using UnityEngine;
using System.Collections;

namespace DNA {

	public class OuterSea : Sea {

		public override float Level {
			get { return average.Level + tide.Level; }
		}

		Renderer _renderer = null;
		Renderer Renderer {
			get {
				if (_renderer == null) {
					_renderer = GetComponent<Renderer> ();
				}
				return _renderer;
			}
		}
		float materialOffset = 0;

		SeaLevel tide = new SeaLevel (0f, 1f);

		void Awake () {
			tide.Swell (60f, true);
		}

		void Update () {

			// funcky texture thing
			Renderer.sharedMaterial.mainTextureOffset = new Vector2 (Mathf.PerlinNoise (materialOffset, 0), 0f);
			materialOffset += 0.001f;

			// funcky texture thing
			Renderer.sharedMaterial.mainTextureOffset = new Vector2 (Mathf.PerlinNoise (materialOffset, 0), 0f);
			materialOffset += 0.001f;

			// update sea y position baseed on sea level and tide
			MyTransform.SetLocalPositionY (Level);
		}
	}
}