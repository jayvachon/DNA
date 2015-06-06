using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GameInput;

namespace GameEvents {
	
	public class ClickEvent : GameEvent {
		
		public readonly List<ClickSettings> clickSettings;

		public ClickEvent (List<ClickSettings> clickSettings) {
			this.clickSettings = clickSettings;
		}

		public bool LayersClicked (InputLayer[] layers) {
			for (int i = 0; i < layers.Length; i ++) {
				if (LayerClicked (layers[i]))
					return true;	
			}
			return false;
		}

		public bool LayerClicked (InputLayer layer) {
			return LayerClickSettings (layer).layerHit;
		}

		public ClickSettings LayerClickSettings (InputLayer layer) {
			return LayerClickSettings ((int)layer);
		}

		public ClickSettings LayerClickSettings (int layer) {
			for (int i = 0; i < clickSettings.Count; i ++) {
				if (clickSettings[i].layer == layer)
					return clickSettings[i];
			}
			return null;
		}
	}
}