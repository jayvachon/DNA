using UnityEngine;

public class PolygonTester : MonoBehaviour {
	void Start () {/*
		// Create Vector2 vertices
		Vector3[] vertices3D = new Vector3[] {
			new Vector3(0,0,0),
			new Vector3(0,50,0),
			new Vector3(50,50,0),
			new Vector3(50,100,0),
			new Vector3(0,100,0),
			new Vector3(0,150,0),
			new Vector3(150,150,100),
			new Vector3(150,100,0),
			new Vector3(100,100,0),
			new Vector3(100,50,0),
			new Vector3(150,50,0),
			new Vector3(150,0,0)
		};
		
		// Use the triangulator to get indices for creating triangles
		Triangulator tr = new Triangulator(vertices3D);
		int[] indices = tr.Triangulate();
		
		// Create the Vector3 vertices
		Vector3[] vertices = new Vector3[vertices3D.Length];
		for (int i=0; i<vertices.Length; i++) {
			vertices[i] = vertices3D[i];//new Vector3(vertices3D[i].x, vertices3D[i].y, vertices3D[i].z);
		}
		
		// Create the mesh
		Mesh msh = new Mesh();
		msh.vertices = vertices;
		msh.triangles = indices;
		msh.RecalculateNormals();
		msh.RecalculateBounds();
		
		// Set up game object with mesh;
		gameObject.AddComponent(typeof(MeshRenderer));
		MeshFilter filter = gameObject.AddComponent(typeof(MeshFilter)) as MeshFilter;
		filter.mesh = msh;*/
	}

	public void Create (Vector3[] vertices3D) {
		// Use the triangulator to get indices for creating triangles
		Triangulator tr = new Triangulator(vertices3D);
		int[] indices = tr.Triangulate();
		
		// Create the Vector3 vertices
		Vector3[] vertices = new Vector3[vertices3D.Length];
		for (int i=0; i<vertices.Length; i++) {
			vertices[i] = vertices3D[i];
		}
		
		// Create the mesh
		Mesh msh = new Mesh();
		msh.vertices = vertices;
		msh.triangles = indices;
		msh.RecalculateNormals();
		msh.RecalculateBounds();
		
		// Set up game object with mesh;
		gameObject.AddComponent(typeof(MeshRenderer));
		MeshFilter filter = gameObject.AddComponent(typeof(MeshFilter)) as MeshFilter;
		filter.mesh = msh;
	}
}