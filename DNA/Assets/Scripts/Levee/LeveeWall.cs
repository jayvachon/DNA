using UnityEngine;
using System.Collections;

namespace DNA {

	public class LeveeWall : MonoBehaviour {

		void Awake () {
			GetComponent<Renderer> ().SetColor (Palette.Tan);
		}
	}
}