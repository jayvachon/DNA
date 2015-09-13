using UnityEngine;
using System.Collections;

namespace DNA.InputSystem {

	public enum InputLayer {
		UI = 5,
		StaticUnits = 8,
		MobileUnits = 9,
		PathPoints = 10,
		Structure = 11,
		Dragging = 12
	}

	public class LayerController {
		static int[] layers = new int[] { 5, 8, 9, 10, 11, 12 };
		public static int[] Layers { get { return layers; } }
	}
}