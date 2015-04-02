using UnityEngine;
using System.Collections;

namespace GameInput {

	public enum InputLayer {
		UI = 5,
		Units = 8,
		PathPoints = 9,
		Structure = 10,
		Dragging = 11
	}

	public class LayerController {

		static int[] layers = new int[] { 5, 8, 9, 10, 11 };
		public static int[] Layers { get { return layers; } }

		static readonly int defaultLayer = (int)InputLayer.Units | (int)InputLayer.Structure | (int)InputLayer.PathPoints;
		static readonly int ignoreLayers = (int)InputLayer.UI;// | (int)InputLayer.Dragging;

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