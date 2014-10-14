using UnityEngine;
using System.Collections;

public static class CustomMesh {

	public static Mesh CreateMesh (Vector3[] vertices, int[] triangles) {
		
		Mesh mesh = new Mesh ();
		mesh.vertices = vertices;
		Vector2[] uvs = new Vector2[vertices.Length];
		for (int i = 0; i < uvs.Length; i ++) {
			uvs[i] = new Vector2 (mesh.vertices[i].x, mesh.vertices[i].z);
		}
		mesh.uv = uvs;
		mesh.triangles = triangles;
		mesh.RecalculateNormals ();
		
		return mesh;
	}

	public static Mesh Hexagon () {

		int sideCount = 6;
		float length = 1f;
		Vector2[] points = new Vector2[sideCount];
		float deg = 360f / (float)sideCount;
		for (int i = 0; i < points.Length; i ++) {
			float radians = (float)i * deg * Mathf.Deg2Rad;
			float x = length * Mathf.Sin (radians);
			float y = length * Mathf.Cos (radians);
			points[i] = new Vector2 (x, y);
		}

		return CustomMesh.CreateMesh (
			new Vector3[] {
				
				// Outer 1
				new Vector3(points[0].x, 0, points[0].y),
				new Vector3(points[1].x, 0, points[1].y),
				new Vector3(points[2].x, 0, points[2].y),
				
				// Outer 2
				new Vector3(points[2].x, 0, points[2].y),
				new Vector3(points[3].x, 0, points[3].y),
				new Vector3(points[4].x, 0, points[4].y),
				
				// Outer 3
				new Vector3(points[4].x, 0, points[4].y),
				new Vector3(points[5].x, 0, points[5].y),
				new Vector3(points[0].x, 0, points[0].y),
				
				// Inner
				new Vector3(points[0].x, 0, points[0].y),
				new Vector3(points[2].x, 0, points[2].y),
				new Vector3(points[4].x, 0, points[4].y)
			},
			new int[12] { 
				0, 1, 2,
				3, 4, 5,
				6, 7, 8,
				9, 10, 11
			}
		);
	}
	
	public static Mesh Step (float width) {

		float halfWidth = width * 0.5f;
		float length = 1f;

		return CustomMesh.CreateMesh (
			new Vector3[] {
				new Vector3(0, 0, 0),
				new Vector3(halfWidth, 0, length),
				new Vector3(-halfWidth, 0, length)	
			},
			new int[3] { 2, 1, 0 }
		);
	}

	public static Mesh Oil () {
		float half = 0.5f;
		return CustomMesh.CreateMesh (
			new Vector3[] {
				new Vector3(0, 0, half),
				new Vector3(-1, 0, -1),
				new Vector3(1, 0, -1)
			},
			new int[3] { 2, 1, 0 }
		);
	}
}


