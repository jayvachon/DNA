using UnityEngine;
using System.Collections;

public class Step : MonoBehaviour {

	public GameObject hexagon;
	Transform myTransform;
	int rowCount = 24;
	float apothem = 0f;
	Hexagon[] hexagons = new Hexagon[0];
	Vector3[] points = new Vector3[0];

	bool ready = false;
	public bool Ready {
		get { return ready; }
	}

	private void Awake () {
		myTransform = transform;
	}

	public void StepInit (float width, float rotation) {

		float[,] terrain = TerrainGenerator.GetTerrain (rowCount);
		Hexagon h = CreateHexagon (myTransform.position, terrain[0, 0]).GetComponent<Hexagon>();
		apothem = h.SideLength / (2f * Mathf.Tan (Mathf.PI / h.SideCount));

		Vector3 position = myTransform.position;
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
		ready = true;
	}

	private GameObject CreateHexagon (Vector3 position, float noiseVal) {
		GameObject go = Instantiate (hexagon, position, Quaternion.identity) as GameObject;
		go.transform.parent = myTransform;
		Hexagon h = go.GetComponent<Hexagon>();
		h.SetType (noiseVal);
		hexagons = ArrayExtended.AppendArray (hexagons, h);
		points = ArrayExtended.AppendArray (points, position);
		return go;
	}

	public Hexagon NearestHexagon (Vector3 position) {
		Hexagon nearestHexagon = null;
		float nearestDistance = Mathf.Infinity;
		for (int i = 0; i < hexagons.Length; i ++) {
			float distance = Vector3.Distance (position, points[i]);
			if (distance < nearestDistance) {
				nearestDistance = distance;
				nearestHexagon = hexagons[i];
				if (distance <= apothem) break;
			}
		}
		return nearestHexagon;
	}

	public Vector3 GetRandomHexagonPosition () {
		return points[Random.Range (0, points.Length)];
	}
}
