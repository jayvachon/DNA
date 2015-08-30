using UnityEngine;
using System.Collections;

public class HappinessIndicator : FloatingIndicator, IPoolable {

	protected override void Awake () {
		SetColor (Color.blue);
	}
}
