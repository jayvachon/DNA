﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DNA.InputSystem;

namespace DNA.Paths {

	public class Road : MBRefs, IPathElementObject {

		public PathElement Element { get; set; }

		void Awake () {
			GetComponent<Renderer> ().SetColor (Color.black);
		}

		public void Init (float length) {
			MyTransform.localScale = new Vector3 (0.1f, 0.1f, length);
		}
	}
}