using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

namespace DNA.EventSystem {

	public class PointerDownEvent : GameEvent {

		public readonly IPointerDownHandler pdh;
		public readonly PointerEventData data;

		public MonoBehaviour ClickedObject {
			get { return pdh as MonoBehaviour; }
		}

		public PointerDownEvent (IPointerDownHandler pdh, PointerEventData data) {
			this.pdh = pdh;
			this.data = data;
		}
	}
}