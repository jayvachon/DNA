using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GameEvents;

namespace GameInput {

	public delegate void OnRegisterClick (ClickSettings clickSettings);

	public class ClickManager {
		
		protected const int LEFT = 0;
		protected const int RIGHT = 1;
		List<ClickHandler> clicks = new List<ClickHandler> ();
		List<ClickSettings> clicksHeard = new List<ClickSettings> ();

		public ClickManager (int[] layers) {
			for (int i = 0; i < layers.Length; i ++) {
				clicks.Add (new ClickHandler (true, layers[i], OnRegisterClick));
				clicks.Add (new ClickHandler (false, layers[i], OnRegisterClick));
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

		void OnRegisterClick (ClickSettings clickSettings) {
			clicksHeard.Add (clickSettings);
			if (clicksHeard.Count == clicks.Count/2) {
				ClickEvent clickEvent = new ClickEvent (clicksHeard);
				Events.instance.Raise (clickEvent);
				Click (clicksHeard[0].left ? LEFT : RIGHT, clickEvent);
				clicksHeard.Clear ();
			}
		}

		void Click (int mouseButton, ClickEvent clickEvent) {
			for (int i = mouseButton; i < clicks.Count; i ++) {
				clicks[i].Click (clickEvent);
			}
		}
	}

	public class ClickHandler : MouseButtonHandler<IClickable> {

		ClickSettings clickSettings;
		OnRegisterClick onRegisterClick;

		public ClickHandler (bool left, int layer, OnRegisterClick onRegisterClick) : base (left, layer) {
			this.onRegisterClick += onRegisterClick;
		}

		protected override void OnDown () {
			clickSettings = new ClickSettings (left, Layer, LayerHit, Moused);
			onRegisterClick (clickSettings);
		}

		public void Click (ClickEvent e) {
			if (Moused != null) {
				if (!e.LayersClicked (Moused.IgnoreLayers)) {
					Moused.OnClick (clickSettings);
				}
			}
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