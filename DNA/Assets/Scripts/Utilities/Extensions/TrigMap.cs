using UnityEngine;
using System.Collections;

public class TrigMap {

	public static float HalfCos01 (float t) {
		// 0 to 1, slowing at the extremes
		return (Mathf.Cos (t * 180f * Mathf.Deg2Rad) - 1) / -2;
	}

	public static float HalfCos (float t) {
		// -1 to 1, slowing at the extremes
		return Mathf.Cos (t * 360f * Mathf.Deg2Rad);
	}

	public static float Sin (float t) {
		return Mathf.Sin (t * 360f * Mathf.Deg2Rad);
	}
}
