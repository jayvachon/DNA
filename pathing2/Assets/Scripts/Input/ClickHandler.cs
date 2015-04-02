using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GameEvents;

namespace GameInput {

	public delegate void OnClick (ClickSettings clickSettings);

	public class ClickManager {

		List<ClickHandler> clicks = new List<ClickHandler> ();
		List<ClickSettings> clicksHeard = new List<ClickSettings> ();

		public ClickManager (int[] layers) {
			for (int i = 0; i < layers.Length; i ++) {
				clicks.Add (new ClickHandler (true, layers[i], OnClick));
				clicks.Add (new ClickHandler (false, layers[i], OnClick));
			}
		}

		public void HandleMouseDown (int mouseButton) {
			for (int i = mouseButton; i < clicks.Count; i += 2) {
				clicks[i].HandleMouseDown ();
			}
		}

		public void HandleMouseUp (int mouseButton) {
			for (int i = mouseButton; i < clicks.Count; i += 2) {
				clicks[i].HandleMouseUp ();
			}
		}

		void OnClick (ClickSettings clickSettings) {
			clicksHeard.Add (clickSettings);
			if (clicksHeard.Count == clicks.Count/2) {
				Events.instance.Raise (new ClickEvent (clicksHeard));
				clicksHeard.Clear ();
			}
		}
	}

	public class ClickHandler : MouseButtonHandler<IClickable> {

		OnClick onClick;

		public ClickHandler (bool left, int layer, OnClick onClick) : base (left, layer) {
			this.onClick += onClick;
		}

		protected override void OnDown () {
			ClickSettings clickSettings = new ClickSettings (left, Layer, LayerHit, Moused);
			if (Moused != null) {
				Moused.OnClick (clickSettings);
			} 
			onClick (clickSettings);
		}
	}

	public class ClickSettings : System.Object {

		public readonly bool left;
		public readonly int layer;
		public readonly bool layerHit;
		public readonly IClickable clickable;

		public ClickSettings (bool left, int layer, bool layerHit, IClickable clickable) {
			this.left = left;
			this.layer = layer;
			this.layerHit = layerHit;
			this.clickable = clickable;
		}
	}
}