using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

namespace DNA.EventSystem {

	public class PointerDownEvent : GameEvent {

		public readonly IPointerDownHandler pdh;

		public MonoBehaviour ClickedObject {
			get { return pdh as MonoBehaviour; }
		}

		public PointerDownEvent (IPointerDownHandler pdh) {
			this.pdh = pdh;
		}
	}
}