using UnityEngine;
using System.Collections;

public class CowActionsList : ActionsList {

	public CowActionsList (Unit unit) : base (unit) {
		SetActions (new Action[] {
			new Action ("Upgrade")
		});
	}
}
