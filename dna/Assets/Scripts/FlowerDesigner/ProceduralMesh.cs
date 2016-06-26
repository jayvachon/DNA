using UnityEngine;
using System.Collections;

namespace DNA.FlowerDesigner {

	public class ProceduralMesh {

		Mesh mesh = null;
		Mesh Mesh {
			get {
				if (mesh == null) {
					mesh = new Mesh ();
				}
				return mesh;
			}
		}

		Vector3[] Vertices {
			get { return Mesh.vertices; }
			set { Mesh.vertices = value; }
		}

		Vector2[] UV {
			get { return Mesh.uv; }
			set { Mesh.uv = value; }
		}

		int[] Triangles {
			get { return Mesh.triangles; }
			set { Mesh.triangles = value; }
		}

		public Mesh SetVertices (Vector3[] vertices) {

			Mesh.Clear ();
			Vertices = vertices;

			Vector2[] v2 = new Vector2[vertices.Length];
			for (int i = 0; i < vertices.Length; i ++) {
				v2[i] = new Vector2 (vertices[i].x, vertices[i].z);
			}

			Triangulator triangulator = new Triangulator (v2);
			Triangles = triangulator.Triangulate ();
			
			Mesh.RecalculateNormals();
			Mesh.RecalculateBounds();
			Mesh.Optimize();

			return Mesh;
		}
	}
}