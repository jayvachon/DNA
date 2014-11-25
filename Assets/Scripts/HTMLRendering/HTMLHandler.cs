using UnityEngine;
using System.Collections;

public class HTMLHandler : MonoBehaviour {

	HTMLRenderer htmlRenderer;

	void Start () {
		htmlRenderer = new HTMLRenderer ("Assets/HTML/test.html");
	}
	
}
