using UnityEngine;
using System.Collections;

public class TerrainGenerator : System.Object {

	public float[,] GetTerrain (int rowCount, int columnCount = -1) {
		if (columnCount == -1)
			columnCount = rowCount;

		float[,] terrain = new float[rowCount + 1, columnCount];
		float xOrg = Random.Range (0f, 1000f);
		float yOrg = Random.Range (0f, 1000f);
		float scale = 3.5f;

		for (int x = 0; x < rowCount + 1; x ++) {
			for (int y = 0; y < columnCount; y ++) {
				float xCoord = xOrg + (x / (float)rowCount + 1) * scale * (rowCount / columnCount);
				float yCoord = yOrg + y / (float)columnCount * scale;
				terrain[x, y] = Mathf.PerlinNoise (xCoord, yCoord);
			}
		}

		return terrain;
	}

	public float[,] GetFlatTerrain (int rowCount, int columnCount = -1) {
		if (columnCount == -1)
			columnCount = rowCount;
		
		float[,] terrain = new float[rowCount + 1, columnCount];
		
		for (int x = 0; x < rowCount + 1; x ++) {
			for (int y = 0; y < columnCount; y ++) {
				terrain[x, y] = 0f;
			}
		}
		
		return terrain;
	}


	public float[,] GetRoundedTerrain (int rowCount, int range) {
		float[,] terrain = GetTerrain (rowCount);
		float r = (float)range + 1f;
		for (int x = 0; x < rowCount + 1; x ++) {
			for (int y = 0; y < rowCount; y ++) {
				terrain[x, y] = Mathf.Round (terrain[x, y] * r);
			}
		}
		return terrain;
	}
}
