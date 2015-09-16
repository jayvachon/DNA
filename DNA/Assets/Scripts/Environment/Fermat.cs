using UnityEngine;
using System.Collections;

public class Fermat {

	public readonly float goldenAngle = 137.508f;

	public struct Settings {
		
		public readonly float radius;
		public readonly int pointCount;
		public readonly float altitude;
		public readonly Vector3 origin;

		public Settings (float radius, int pointCount, float altitude, Vector3 origin) {
			this.radius = radius;
			this.pointCount = pointCount;
			this.altitude = altitude;
			this.origin = origin;
		}
	}

	Settings settings = new Settings (3f, 200, 0.2f, Vector3.zero);

	Vector3[] points;
	public Vector3[] Points {
		get {

			points = new Vector3[settings.pointCount];

			for (int i = 0; i < points.Length; i ++) {
				float angle = (float)i * goldenAngle * Mathf.Deg2Rad;
				float distance = settings.radius * Mathf.Sqrt (angle);
				points[i] = new Vector3 (
					settings.origin.x + distance * Mathf.Sin (angle),
					settings.origin.y + i * -settings.altitude,
					settings.origin.z + distance * Mathf.Cos (angle)
				);
			}

			return points;
		}
	}

	public Fermat (Settings? settings=null) {
		if (settings != null)
			this.settings = (Settings)settings;
	}

	int Fibonacci (int n) {
		if (n <= 0)
			return 0;
		if (n == 1)
			return 1;
		return Fibonacci (n-1) + Fibonacci (n-2);
	}

	public Vector3[] UpdateSettings (Settings settings) {
		if (!this.settings.Equals (settings)) {
			this.settings = settings;
			return Points;
		}
		return points;
	}
}