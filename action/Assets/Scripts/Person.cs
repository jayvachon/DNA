using UnityEngine;
using System.Collections;
using GameActions;

public class Person : Unit, IActionable {

	public ActionsList ActionsList { get; set; }

	void Awake () {
		ActionsList = new ActionsList ();
		ActionsList.Add (new CollectIceCream ());
	}
}
