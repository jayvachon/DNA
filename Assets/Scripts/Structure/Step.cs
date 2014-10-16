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

		float[,] terrain = TerrainGenerator.GetTerrain (rowCount);
		Hexagon h = CreateHexagon (myTransform.position, terrain[0, 0]).GetComponent<Hexagon>();

		Vector3 position = myTransform.position;
		float apothem = h.SideLength / (2f * Mathf.Tan (Mathf.PI / h.SideCount));
		float sideSep = h.SideLength * 1.5f;
		int rowLength = 2;
		for (int y = 0; y < rowCount; y ++) {
			position.z = sideSep * (y + 1);
			float halfRowLength = rowLength * 0.5f;
			for (int x = 0; x < rowLength; x ++) {
				position.x = x * apothem * 2f;
				position.x -= halfRowLength * apothem * 2f;
				position.x += apothem;
				CreateHexagon (position, terrain[x, y]);
			}
			rowLength ++;
		}

		myTransform.eulerAngles = new Vector3 (0, rotation, 0);
	}

	private GameObject CreateHexagon (Vector3 position, float noiseVal) {
		GameObject go = Instantiate (hexagon, position, Quaternion.identity) as GameObject;
		go.transform.parent = myTransform;
		go.GetComponent<Hexagon>().SetType (noiseVal);
		return go;
	}
}
