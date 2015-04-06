using UnityEngine;
using System.Collections;

public delegate void OnAge (float progress);
public delegate void OnRetirement ();

public class AgeManager : Interval {

	float retirementAge = 180;	// seconds until becoming an elder
	OnAge onAge;
	OnRetirement onRetirement;

	bool retired = false;
	public bool Retired {
		get { return retired; }
	}

	public void BeginAging (OnAge onAge, OnRetirement onRetirement) {
		this.onAge = onAge;
		this.onRetirement = onRetirement;
		Begin (retirementAge);
	}

	public override void Run (float progress) {
		onAge (progress);
	}

	public override void End () {
		retired = true;
		onRetirement ();
	}
}
