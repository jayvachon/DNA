using UnityEngine;
using System.Collections;

namespace DNA.FlowerDesigner {

	public static class ProceduralMesh {

		public static Mesh Generate (Vector3[] vertices) {
			int[] triangles = new int[vertices.Length];
			for (int i = 0; i < triangles.Length; i ++) {
				triangles[i] = i;
			}
			return Generate (vertices, triangles);
		}

		public static Mesh Generate (Vector3[] vertices, int[] triangles) {

			Mesh mesh = new Mesh ();
			mesh.vertices = vertices;
			Vector2[] uvs = new Vector2[vertices.Length];
			for (int i = 0; i < uvs.Length; i ++) {
				uvs[i] = new Vector2 (mesh.vertices[i].x, mesh.vertices[i].z);
			}
			mesh.uv = uvs;
			mesh.triangles = triangles;
			mesh.RecalculateNormals ();
			mesh.RecalculateBounds();
			mesh.Optimize ();
			
			return mesh;
		}
	}

	public static class ProceduralMeshExtensionMethods {

		public static void GenerateMesh (this GameObject gameObject, Vector3[] vertices, int[] triangles, bool addCollider=false) {
			
			MeshRenderer mr = gameObject.LazyGetComponent<MeshRenderer> ();
			MeshFilter mf = gameObject.LazyGetComponent<MeshFilter> ();
			MeshCollider mc = gameObject.GetComponent<MeshCollider> ();

			mf.mesh = ProceduralMesh.Generate (vertices, triangles);

			if (addCollider) {
				if (mc == null)
					mc = gameObject.AddComponent<MeshCollider> ();
				mc.sharedMesh = mf.mesh;
			}

		}
	}
}