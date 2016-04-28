using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using DNA.InputSystem;
using DNA.Paths;

namespace DNA.Units {

	public class Road : StaticUnit {

		public PathElement Element { get; set; }

		void Awake () {
			GetComponent<Renderer> ().SetColor (Palette.Yellow);
		}

		public void Init (float length) {
			MyTransform.localScale = new Vector3 (0.35f, 0.1f, length);
		}

		protected override void OnEnable () {}
	}
}