using UnityEngine;
using System.Collections;

public class Hospital : Building {

	Color defaultColor = Color.white;
	Color selectColor = Color.grey;

	public override void OnStart () {
		renderer.SetColor (defaultColor);
		Init (
			new string[] {"1", "2"},
			new string[] {"make a person (10 milkshakes)", "make a cow (20 milkshakes)"}
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
			case "1": CreatePerson (); break;
			case "2": CreateCow (); break;
		}
	}

	void CreatePerson () {
		if (GM.instance.SubMshake (10)) {
			Vector3 creationPosition = MyTransform.position;
			creationPosition.x += MyTransform.localScale.x + 0.25f;
			Events.instance.Raise (new CreatePersonEvent (creationPosition));
		}
	}

	void CreateCow () {
		if (GM.instance.SubMshake (20)) {
			Vector3 creationPosition = MyTransform.position;
			creationPosition.z += MyTransform.localScale.z + 0.25f;
			Events.instance.Raise (new CreateCowEvent (creationPosition));
		}
	}
}
