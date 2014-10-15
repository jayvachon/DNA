using UnityEngine;
using System.Collections;

public class Step : MonoBehaviour {

	public GameObject hexagon;
	Transform myTransform;
	int rowCount = 24;

	private void Awake () {
		myTransform = transform;
	}

	public void StepInit (float width, float rotation) {

		Hexagon h = CreateHexagon (myTransform.position).GetComponent<Hexagon>();

		Vector3 position = myTransform.position;
		float apothem = h.SideLength / (2f * Mathf.Tan (Mathf.PI / h.SideCount));
		float sideSep = h.SideLength * 1.5f;
		int rowLength = 2;
		for (int i = 0; i < rowCount; i ++) {
			position.z = sideSep * (i + 1);
			float halfRowLength = rowLength * 0.5f;
			for (int j = 0; j < rowLength; j ++) {
				position.x = j * apothem * 2f;
				position.x -= halfRowLength * apothem * 2f;
				position.x += apothem;
				CreateHexagon (position);
			}
			rowLength ++;
		}

		myTransform.eulerAngles = new Vector3 (0, rotation, 0);
	}

	private GameObject CreateHexagon (Vector3 position) {
		GameObject go = Instantiate (hexagon, position, Quaternion.identity) as GameObject;
		go.transform.parent = myTransform;
		return go;
	}
}
