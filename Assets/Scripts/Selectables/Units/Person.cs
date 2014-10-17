using UnityEngine;
using System.Collections;

public class Person : Unit {

	Color defaultColor = Color.red;
	Color selectedColor = Color.magenta;

	public override void OnStart () {
		renderer.SetColor (defaultColor);
	}

	public override void OnSelect () {
		renderer.SetColor (selectedColor);
	}
	
	public override void OnUnselect () {
		renderer.SetColor (defaultColor);
	}
}