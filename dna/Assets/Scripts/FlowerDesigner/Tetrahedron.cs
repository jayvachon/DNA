using UnityEngine;
using System.Collections;

namespace DNA.FlowerDesigner {

	public class Tetrahedron : MonoBehaviour {

		void Start () {

			Vector3 p0 = new Vector3(0,0,0);
			Vector3 p1 = new Vector3(1,0,0);
			Vector3 p2 = new Vector3(0,0,1);
			// Vector3 p2 = new Vector3(0.5f,0,Mathf.Sqrt(0.75f));
			// Vector3 p3 = new Vector3(0.5f,Mathf.Sqrt(0.75f),Mathf.Sqrt(0.75f)/3);

			gameObject.GetComponent<MeshFilter> ().mesh = new ProceduralMesh ().SetVertices (new Vector3[]{
				p0,p1,p2
			});
		}
	}
}