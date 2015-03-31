using UnityEngine;
using System.Collections;

namespace GameInput {

	public enum InputLayer {
		UI = 1 << 5,
		Units = 1 << 8,
		PathPoints = 1 << 9,
		Structure = 1 << 10,
		Dragging = 1 << 11
	}

	public class LayerController {

		static readonly int defaultLayer = (int)InputLayer.Units | (int)InputLayer.Structure;
		static readonly int ignoreLayers = (int)InputLayer.UI | (int)InputLayer.Dragging;

		public static int DefaultLayer {
			get { return defaultLayer; }
		}

		public static int IgnoreLayers {
			get { return ignoreLayers; }
		}

		public static InputLayer Layer {
			set { MouseController.Layer = (int)value; }
		}

		public static void Reset () {
			MouseController.Layer = defaultLayer;
		}
	}
}