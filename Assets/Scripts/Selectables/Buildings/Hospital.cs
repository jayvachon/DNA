using UnityEngine;
using System.Collections;

public class Hospital : Building {

	Color defaultColor = Color.white;
	Color selectColor = Color.grey;

	public override void OnStart () {
		renderer.SetColor (defaultColor);
		Init (
			new string[] {"1", "2"},
			new string[] {"make a person", "make a shitstain"}
		);
	}

	public override void OnSelect () {
		renderer.SetColor (selectColor);
	}

	public override void OnUnselect () {
		renderer.SetColor (defaultColor);
	}

	public override void OnKeyPress (string key) {
		switch (key) {
			case "1": Debug.Log ("fuckin right"); break;
			case "2": Debug.Log ("eerrf shit"); break;
		}
	}
}
