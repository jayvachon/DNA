using UnityEngine;
using System.Collections;

public class HealthIndicator : FloatingIndicator {

	protected override void Awake () {
		SetColor (Color.red);
	}
}
