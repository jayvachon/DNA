using UnityEngine;
using System.Collections;

public class Hospital : Building {

	void Start () {
		pointAction = new BuildAction (3f);
		actionsList = new HospitalActionsList (this);
		renderer.SetColor (Color.grey);
	}

	public override void OnPerformAction (Action action) {
		Debug.Log (action.name);
	}
}