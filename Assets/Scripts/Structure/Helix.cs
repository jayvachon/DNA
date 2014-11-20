using UnityEngine;
using System.Collections;

public class Helix {
	
	public readonly Vector3 center = new Vector3 (0, 0, 0);
	public readonly int sideCount = 6;
	public readonly int rotations = 10;
	public readonly int rise = 100;
	public readonly float pointRise;
	public readonly float sideLength;
	public readonly Vector3[] points;
	public readonly float[] pointRotations;
	public readonly int radius = 100;
	
	public Helix (Vector3 center, int rotations = 2, int sideCount = 10, int radius = 200, int rise = 100) {
		this.center = center;
		this.sideCount = sideCount;
		this.rotations = rotations;
		this.radius = radius;
		this.rise = rise;
		points = new Vector3[sideCount * rotations];
		pointRotations = new float[sideCount * rotations];
		pointRise = (float)rise / (float)sideCount;
		CreateHelix ();
		sideLength = Vector2.Distance (new Vector2 (points[0].x, points[0].z), new Vector2 (points[1].x, points[1].z));
	}
	
	void CreateHelix () {
		float deg = 360f / (float)sideCount;
		for (int i = 0; i < points.Length; i ++) {
			float radians = (float)i * deg * Mathf.Deg2Rad;
			points[i] = new Vector3 (
				center.x + radius * Mathf.Sin (radians),
				center.y + i * pointRise,
				center.z + radius * Mathf.Cos (radians)
			);
			pointRotations[i] = i * deg;
		}
	}
	
	bool PointOnLineSegment (Vector3 pt1, Vector3 pt2, Vector3 pt, float epsilon = 10f) {
		if (pt.x - Mathf.Max (pt1.x, pt2.x) >= epsilon || 
		    Mathf.Min (pt1.x, pt2.x) - pt.x >= epsilon || 
		    pt.z - Mathf.Max (pt1.z, pt2.z) >= epsilon ||
		    Mathf.Min (pt1.z, pt2.z) - pt.z >= epsilon)
			return false;
		
		if (Mathf.Abs (pt2.x - pt1.x) < epsilon)
			return Mathf.Abs (pt1.x - pt.x) < epsilon || Mathf.Abs (pt2.x - pt.x) < epsilon;
		if (Mathf.Abs (pt2.z - pt1.z) < epsilon)
			return Mathf.Abs (pt1.z - pt.z) < epsilon || Mathf.Abs(pt2.z - pt.z) < epsilon;
		
		float x = pt1.x + (pt.z - pt1.z) * (pt2.x - pt1.x) / (pt2.z - pt1.z);
		float z = pt1.z + (pt.x - pt1.x) * (pt2.z - pt1.z) / (pt2.x - pt1.x);
		
		return Mathf.Abs (pt.x - x) < epsilon || Mathf.Abs (pt.z - z) < epsilon;
	}
	
	float GetYOnLineSegment (Vector3 pt1, Vector3 pt2, Vector3 pt) {
		float dis = Vector2.Distance (new Vector2 (pt1.x, pt1.z), new Vector2 (pt.x, pt.z));
		return pt1.y + (pt2.y - pt1.y) * (dis / sideLength);
	}
	
	public float PointOnHelix (Vector3 pt, float epsilon = 50f) {
		for (int i = 0; i < points.Length - 1; i ++) {
			if (PointOnLineSegment (points[i], points[i + 1], pt, epsilon))
				return GetYOnLineSegment (points[i], points[i + 1], pt);
		}
		return -1f;
	}
	
	public void DrawHelix () {
		for (int i = 0; i < points.Length - 1; i ++) {
			Debug.DrawLine (points[i], points[i + 1], Color.red);
		}
	}
}