using UnityEngine;
using System.Collections;

public class Helix {

	// idk math so these terms are probably wrong oh well
	public readonly Vector3 center 	= new Vector3 (0, 0, 0);
	public readonly int resolution 	= 5;  // number of points in a single rotation
	public readonly int rotations	= 3;  // number of rotations (a rotation is a full 360 degree turn)
	public readonly float radius	= 10; // distance from center to side
	public readonly float height	= 2;  // distance between a side and the side one rotation above or below it

	Vector4[] points;
	public Vector4[] Points {
		get { return points; }
	}
	
	public Helix (float radius, float height) {
		this.radius = radius;
		this.height = height;
		CreateHelix ();
	}

	void CreateHelix () {
		
		points = new Vector4[resolution * rotations];
		float deg = 360f / (float)resolution;

		for (int i = 0; i < points.Length; i ++) {
			float radians = (float)i * deg * Mathf.Deg2Rad;
			points[i] = new Vector4 (
				center.x + radius * Mathf.Sin (radians),
				center.y + i * height,
				center.z + radius * Mathf.Cos (radians),
				i * deg
			);
		}
	}

	public void Draw () {
		for (int i = 0; i < points.Length - 1; i ++) {
			Debug.DrawLine (points[i], points[i + 1], Color.red);
		}
	}
}
