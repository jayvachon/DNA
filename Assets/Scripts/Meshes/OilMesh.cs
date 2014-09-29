using UnityEngine;
using System.Collections;

public static class OilMesh {

	public static Mesh CreateMesh () {
		
		Mesh mesh = new Mesh ();
		
		// Vertices
		float half = 0.5f;
		mesh.vertices = new Vector3[] {
			new Vector3(0, 0, half),
			new Vector3(-1, 0, -1),
			new Vector3(1, 0, -1)
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
