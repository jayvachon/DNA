using UnityEngine;
using System.Collections;

namespace DNA {

	public class OuterSea : Sea2 {

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
		
		protected override void Awake () {
			base.Awake ();
		}

		void Update () {
			Renderer.sharedMaterial.mainTextureOffset = new Vector2 (Mathf.PerlinNoise (materialOffset, 0), 0f);
			materialOffset += 0.001f;
		}
	}
}