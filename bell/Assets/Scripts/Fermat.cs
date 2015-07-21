using UnityEngine;
using System.Collections;

public class Fermat {

	public readonly Vector3 origin = Vector3.zero;
	public readonly float goldenAngle = 137.508f;

	public struct Settings {
		
		public readonly float radius;
		public readonly int pointCount;
		public readonly float altitude;

		public Settings (float radius, int pointCount, float altitude) {
			this.radius = radius;
			this.pointCount = pointCount;
			this.altitude = altitude;
		}

		/*public static bool operator ==(Settings a, Settings b) {
			//return a.Equals (b);
			return a.radius == b.radius 
				&& a.pointCount == b.pointCount 
				&& a.altitude == b.altitude;
		}*/

		/*public static bool operator !=(Settings a, Settings b) {
			//return !a.Equals (b);
			return a.radius != b.radius 
				|| a.pointCount != b.pointCount 
				|| a.altitude != b.altitude;
		}*/
	}

	Settings settings = new Settings (3f, 200, 0.2f);

	Vector3[] points;
	public Vector3[] Points {
		get {

			points = new Vector3[settings.pointCount];

			for (int i = 0; i < points.Length; i ++) {
				float angle = (float)i * goldenAngle * Mathf.Deg2Rad;
				float distance = settings.radius * Mathf.Sqrt (angle);
				points[i] = new Vector3 (
					origin.x + distance * Mathf.Sin (angle),
					origin.y + i * -settings.altitude,
					origin.z + distance * Mathf.Cos (angle)
				);
			}

			return points;
		}
	}

	int Fibonacci (int n) {
		if (n <= 0)
			return 0;
		if (n == 1)
			return 1;
		return Fibonacci (n-1) + Fibonacci (n-2);
	}

	public Vector3[] UpdateSettings (Settings settings) {
		//if (this.settings != settings) {
		if (!this.settings.Equals (settings)) {
			this.settings = settings;
			return Points;
		}
		return points;
	}
}