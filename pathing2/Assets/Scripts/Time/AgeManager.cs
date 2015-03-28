using UnityEngine;
using System.Collections;

public delegate void OnRetirement ();

public class AgeManager : Interval {

	float age = 0; 				// seconds since birth
	float retirementAge = 60;	// seconds until becoming an elder
	OnRetirement onRetirement;

	public void BeginAging (OnRetirement onRetirement) {
		this.onRetirement = onRetirement;
		Begin (retirementAge);
	}

	public override void End () {
		onRetirement ();
	}
}
