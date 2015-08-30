using UnityEngine;
using System.Collections;

public class GridPoint : MonoBehaviour {

	Vector3 coord;
	Vector3 position;

	public void Create (Vector3 coord, Vector3 position) {
		this.coord = coord;
		this.position = position;
	}
}
