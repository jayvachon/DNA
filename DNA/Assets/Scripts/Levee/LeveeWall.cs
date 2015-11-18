﻿using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using DNA.InputSystem;

namespace DNA {

	public class LeveeWall : MonoBehaviour, IPointerDownHandler {

		public Levee Levee { get; set; }

		void Awake () {
			GetComponent<Renderer> ().SetColor (Palette.Tan);
		}

		#region IPointerDownHandler implementation
		public void OnPointerDown (PointerEventData e) {
			SelectionHandler.ClickSelectable (Levee, e);
		}
		#endregion
	}
}