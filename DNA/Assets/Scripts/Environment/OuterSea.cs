using UnityEngine;
using System.Collections;

namespace DNA {

	public class OuterSea : Sea2 {

		protected override void Awake () {
			base.Awake ();
			foreach (Transform child in MyTransform) {
				child.GetComponent<Renderer> ().SetColor (Palette.Blue);
			}
		}
	}
}