using UnityEngine;
using System.Collections;

public class MercuryRender : MonoBehaviour {

	public void SetColor (Color color) {
		GetComponent<Renderer>().SetColor (color);
	}
}
