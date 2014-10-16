using UnityEngine;
using System.Collections;

public static class TerrainGenerator {

	/*void Update () {
		if (Input.GetKeyDown (KeyCode.Space)) {
			float[,] terrain = GetRoundedTerrain (24, 3);
			for (int x = 0; x < 25; x ++) {
				for (int y = 0; y < 24; y ++) {
					Debug.Log (terrain[x, y]);
				}
			}
		}
	}*/

	public static float[,] GetTerrain (int rowCount) {

		float[,] terrain = new float[rowCount + 1, rowCount];
		float xOrg = Random.Range (0f, 1000f);
		float yOrg = Random.Range (0f, 1000f);
		float scale = 5f;

		for (int x = 0; x < rowCount + 1; x ++) {
			for (int y = 0; y < rowCount; y ++) {
				float xCoord = xOrg + (x / (float)rowCount + 1) * scale;
				float yCoord = yOrg + y / (float)rowCount * scale;
				terrain[x, y] = Mathf.PerlinNoise (xCoord, yCoord);
			}
		}

		return terrain;
	}

	public static float[,] GetRoundedTerrain (int rowCount, int range) {
		float[,] terrain = TerrainGenerator.GetTerrain (rowCount);
		float r = (float)range + 1f;
		for (int x = 0; x < rowCount + 1; x ++) {
			for (int y = 0; y < rowCount; y ++) {
				terrain[x, y] = Mathf.Round (terrain[x, y] * r);
			}
		}
		return terrain;
	}
}
