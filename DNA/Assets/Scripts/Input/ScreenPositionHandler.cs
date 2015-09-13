using UnityEngine;
using System.Collections;

namespace DNA.InputSystem {

	public static class ScreenPositionHandler {

		public static float PointDirection (Vector3 a, Vector3 b) {
			return PointDirection (WorldToScreen(a), WorldToScreen (b));
		}

		public static float PointDirection (Vector2 a, Vector2 b) {
			Vector2 c = a - b;
			return Mathf.Atan2 (c.y, c.x) * Mathf.Rad2Deg;
		}

		public static Vector2 WorldToScreen (Vector3 a) {
			return Camera.main.WorldToScreenPoint (a);
		}

		public static Vector3 ScreenToWorld (Vector2 a) {
			return Camera.main.ScreenToWorldPoint (new Vector3 (a.x, a.y, Camera.main.nearClipPlane));
		}

		public static float AngleDifference (float a, float b) {
			float c = Mathf.Abs (a - b);
			if (c > 180) {
				c -= 360;
				c = Mathf.Abs (c);
			}
			return c;
		}

		public static bool AnglesInRange (float a, float b, float range) {
			return AngleDifference (a, b) < range;
		}
	}
}