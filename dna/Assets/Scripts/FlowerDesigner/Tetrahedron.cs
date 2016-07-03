using UnityEngine;
using System.Collections;

namespace DNA.FlowerDesigner {

	public class Tetrahedron : MonoBehaviour {

		Vector3 p0 = new Vector3(0,0,0);
		Vector3 p1 = new Vector3(1,0,0);
		Vector3 p2 = new Vector3(0.5f,0,Mathf.Sqrt(0.75f));
		Vector3 p3 = new Vector3(0.5f,Mathf.Sqrt(0.75f),Mathf.Sqrt(0.75f)/3);

		void Start () {

			gameObject.GenerateMesh (new Vector3[] { p0, p1, p2, p3 }, new int[] {
				0,1,2,
				0,2,3,
				2,1,3,
				0,3,1
			});
		}
	}
}