using UnityEngine;
using System.Collections;

public class HospitalActionsList : ActionsList {

	public HospitalActionsList (Unit unit) : base (unit) {
		SetActions (new Action[] {
			new Action ("Create person"),
			new Action ("Upgrade")
		});
	}
}
