using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using DNA.InputSystem;
using DNA.EventSystem;

namespace DNA {

	public class LeveeWall : MonoBehaviour, IPointerDownHandler {

		public Levee Levee { get; set; }

		void Awake () {
			GetComponent<Renderer> ().SetColor (Palette.DarkBlue);
		}

		#region IPointerDownHandler implementation
		public void OnPointerDown (PointerEventData e) {
			SelectionHandler.ClickSelectable (Levee, e);
			Events.instance.Raise (new PointerDownEvent (this));
		}
		#endregion
	}
}