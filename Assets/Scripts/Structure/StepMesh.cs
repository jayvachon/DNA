using UnityEngine;
using System.Collections;

public static class StepMesh {

	public static Mesh CreateMesh (float width) {
		
		Mesh mesh = new Mesh ();
		
		// Vertices
		float halfWidth = width * 0.5f;
		float length = 1.0f;
		mesh.vertices = new Vector3[] {
			new Vector3(0, 0, 0),
			new Vector3(halfWidth, 0, length),
			new Vector3(-halfWidth, 0, length)
		};
		
		// UVs
		Vector2[] uvs = new Vector2[3];
		for (int i = 0; i < uvs.Length; i ++) {
			uvs[i] = new Vector2 (mesh.vertices[i].x, mesh.vertices[i].z);
		}
		mesh.uv = uvs;
		
		// Triangles
		mesh.triangles = new int[3] { 2, 1, 0 };
		
		mesh.RecalculateNormals ();
		
		return mesh;
	}
}
