using UnityEngine;
using System.Collections;

public class HealthIndicator : FloatingIndicator, IPoolable {

	protected override void Awake () {
		SetColor (Color.red);
	}
}
