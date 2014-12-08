using UnityEngine;
using System.Collections;

public class Hospital : Building {

	void Start () {
		pointAction = new BuildAction (3f);
		renderer.SetColor (Color.grey);
	}
}