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
	}
}