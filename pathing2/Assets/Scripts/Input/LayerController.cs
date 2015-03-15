using UnityEngine;
using System.Collections;

namespace GameInput {

	public enum InputLayer {
		Units = 8,
		PathPoints = 9
	}

	public class LayerController {

		public static readonly int defaultLayer = 8;

		public static InputLayer Layer {
			set { MouseController.Layer = (int)value; }
		}
	}
}