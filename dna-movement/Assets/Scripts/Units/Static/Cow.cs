using UnityEngine;
using System.Collections;

public class Cow : Building {

	void Start () {
		pointAction = new BuildAction (2f);
		actionsList = new CowActionsList (this);
		renderer.SetColor (Color.black);
	}
}
