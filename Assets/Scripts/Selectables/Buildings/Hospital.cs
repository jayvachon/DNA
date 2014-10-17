using UnityEngine;
using System.Collections;

public class Hospital : Building {

	Color defaultColor = Color.white;
	Color selectColor = Color.grey;

	public override void OnStart () {
		renderer.SetColor (defaultColor);
	}

	public override void OnSelect () {
		renderer.SetColor (selectColor);
	}

	public override void OnUnselect () {
		renderer.SetColor (defaultColor);
	}
}
