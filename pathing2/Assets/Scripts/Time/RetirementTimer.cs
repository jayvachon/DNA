using UnityEngine;
using System.Collections;

public class RetirementTimer : Interval {

	float retirementAge = TimerValues.Retirement;	// seconds until becoming an elder
	public float RetirementAge {
		get { return retirementAge; }
	}

	bool retired = false;
	public bool Retired {
		get { return retired; }
	}

	public void BeginAging (OnRun onAge, OnTimerEnd onRetirement) {
		onRun += onAge;
		onTimerEnd += onRetirement;
		Begin (retirementAge);
	}

	public override void End () {
		base.End ();
		retired = true;
	}
}
