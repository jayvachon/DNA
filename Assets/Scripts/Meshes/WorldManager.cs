using UnityEngine;
using System.Collections;

public class WorldManager : MonoBehaviour {

	public GameObject hexagon;
	struct HexProperties {
		public float radius;
		public float apothem;
		public float sideLength;
		public float circumradius;
		public float heightInterval;
	};
	HexProperties hexProps;

	Hexagon2[] hexagons = new Hexagon2[0];
	Vector3[] coordinates = new Vector3[0];

	void Awake () {
		GetHexProperties ();
		GenerateMap ();
	}

	void GetHexProperties () {
		GameObject go = Instantiate (hexagon, Vector3.zero, Quaternion.identity) as GameObject;
		Hexagon2 h = go.GetComponent<Hexagon2>();
		hexProps.radius = h.Circumradius / h.SideCount;
		hexProps.apothem = h.Apothem;
		hexProps.sideLength = h.SideLength;
		hexProps.circumradius = h.Circumradius;
		hexProps.heightInterval = h.HeightInterval;
		Destroy (go);
	}

	GameObject CreateHexagon (Vector3 coordinate, Vector3 position, float noiseVal) {
		GameObject go = Instantiate (hexagon, position, Quaternion.identity) as GameObject;
		Hexagon2 h = go.GetComponent<Hexagon2>();
		h.Init (coordinate, position, noiseVal, this);
		hexagons = ArrayExtended.AppendArray (hexagons, h);
		coordinates = ArrayExtended.AppendArray (coordinates, coordinate);
		return go;
	}

	void GenerateMap () {

		int xSize = 3;
		int ySize = 3;
		float odd = 0;
		float sideSep = hexProps.sideLength * 1.5f;
		float[,] terrain = TerrainGenerator.GetTerrain (10);

		for (int y = 0; y < ySize; y ++) {
			odd = (y % 2) == 0 ? 1 : 0;
			for (int x = 0; x < xSize; x ++) {
				float noiseVal = terrain[x, y];
				Vector3 position = new Vector3 (x * hexProps.apothem * 2f + (odd * hexProps.apothem), noiseVal * hexProps.heightInterval, sideSep * (y + 1));
				Vector3 coordinate = new Vector3 (x, 0, y);
				CreateHexagon (coordinate, position, noiseVal);
			}
		}

		for (int i = 0; i < hexagons.Length; i ++) {
			hexagons[i].UpdateHeights ();
		}
	}

	public Hexagon2 GetHexagonAtCoordinate (Vector3 coordinate) {
		for (int i = 0; i < coordinates.Length; i ++) {
			if (coordinates[i] == coordinate) {
				return hexagons[i];
			}
		}
		return null;
	}
}
