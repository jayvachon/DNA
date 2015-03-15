using UnityEngine;
using System.Collections;

namespace GameInput {

	public enum InputLayer {
		UI = 1 << 5,
		Units = 1 << 8,
		PathPoints = 1 << 9
	}

	public class LayerController {

		static readonly int uiLayer = (int)InputLayer.UI;
		static readonly int defaultLayer = (int)InputLayer.Units;

		public static int DefaultLayer {
			get { return defaultLayer; }
		}

		public static int IgnoreLayers {
			get { return uiLayer; }
		}

		public static InputLayer Layer {
			set { MouseController.Layer = (int)value; }
		}
	}
}