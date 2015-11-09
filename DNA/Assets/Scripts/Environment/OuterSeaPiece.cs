using UnityEngine;
using System.Collections;

public class OuterSeaPiece : MBRefs {

	void Awake () {
		GetComponent<Renderer> ().SetColor (Palette.Magenta);
	}
}
