using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DNA.InputSystem;

namespace DNA.EventSystem {
	
	public class ClickEvent : GameEvent {
		
		public readonly List<ClickSettings> clickSettings;
		public readonly bool left;

		public ClickEvent (List<ClickSettings> clickSettings) {
			this.clickSettings = clickSettings;
			this.left = clickSettings[0].left;
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
			return clickSettings.Find (x => x.layer == layer);
		}

		public T GetClickedOfType<T> () where T : IClickable {
			// TODO: use IsAssignableFrom
			ClickSettings c = clickSettings.Find (x => x.clickable != null && x.clickable.GetType () == typeof (T));
			return c == null ? default (T) : (T)c.clickable;
		}
	}
}