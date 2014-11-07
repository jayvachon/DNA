using UnityEngine;
using System.Collections;

public class WorldManager2 : MonoBehaviour {

	public GameObject cube;
	public CustomMeshObject cmo;

	int xSize = 100;
	int ySize = 100;
	float cellSize = 10f;
	float heightInterval = 100f;
	Vector3[,] coords;
	Vector3[,] positions;

	float tempH = 0f;

	void Awake () {
		CreateGrid ();
		SetHeight ();
		InstantiateCubes ();
		CreateHexagon ();
	}

	void CreateGrid () {
		coords = new Vector3[xSize, ySize];
		positions = new Vector3[xSize, ySize];
		int odd = 0;
		for (int x = 0; x < xSize; x ++) {
			odd = (x % 2) == 0 ? 1 : 0;
			for (int y = 0; y < ySize; y ++) {
				coords[x, y] = new Vector3 (x, 0, y);
				positions[x, y] = new Vector3 (x * cellSize, 0, y * cellSize - (odd * cellSize * 0.5f));
			}
		}
	}

	void SetHeight () {
		float[,] heightmap = TerrainGenerator.GetTerrain (xSize);
		for (int x = 0; x < xSize; x ++) {
			for (int y = 0; y < ySize; y ++) {
				coords[x, y].y = heightmap[x, y];
				positions[x, y].y = heightmap[x, y] * heightInterval;
			}
		}
	}

	void InstantiateCubes () {
		for (int x = 0; x < xSize; x ++) {
			for (int y = 0; y < ySize; y ++) {
				Instantiate (cube, positions[x, y], Quaternion.identity);
			}
		}
	}

	void CreateHexagon () {
		cmo = Instantiate (cmo, positions[1, 1], Quaternion.identity) as CustomMeshObject;
		float[] heights = new float[6];
		float myHeight = positions[1, 1].y;
		heights[0] = (positions[1, 2].y - myHeight) / 10f;
		heights[1] = (positions[2, 2].y - myHeight) / 10f;
		heights[2] = (positions[0, 0].y - myHeight) / 10f;
		heights[3] = (positions[1, 0].y - myHeight) / 10f;
		heights[4] = (positions[0, 1].y - myHeight) / 10f;
		heights[5] = (positions[0, 2].y - myHeight) / 10f;
		cmo.Init (CustomMesh.Hexagon (heights), Color.green, true);
	}
}
