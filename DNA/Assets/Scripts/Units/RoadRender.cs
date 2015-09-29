using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using DNA.InputSystem;

namespace DNA.Paths {

	public class RoadRender : MBRefs, /*IClickable, IHoverable, */IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler {

		public virtual InputLayer[] IgnoreLayers {
			get { return new InputLayer[] { InputLayer.UI }; }
		}

		Road road = null;
		Road Road {
			get {
				if (road == null) {
					road = Parent.GetComponent<Road> ();
				}
				return road;
			}
		}

		public void OnHoverEnter () {
			//Road.OnHoverEnter ();
		}

		public void OnHoverExit () {
			//Road.OnHoverExit ();
		}

		public void OnClick (ClickSettings clickSettings) {
			//if (clickSettings.left)
				//Road.OnClick ();
		}

		public void OnPointerEnter (PointerEventData e) {
			Road.OnHoverEnter ();
		}

		public void OnPointerExit (PointerEventData e) {
			Road.OnHoverExit ();
		}

		public void OnPointerDown (PointerEventData e) {
			Road.OnClick ();
		}

		public void OnHover () {}
	}
}