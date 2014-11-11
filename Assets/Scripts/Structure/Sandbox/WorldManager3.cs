using UnityEngine;
using System.Collections;

public class WorldManager3 : MonoBehaviour {

	public GridPoint gridPoint;

	Helix helix;
	TriangleGrid triGrid;

	void Start () {
		helix = new Helix (Vector3.zero, 2, 100);
		triGrid = new TriangleGrid ();
		Vector3[,,] points = triGrid.CreateGridOnHelix (helix, 13);
		int xLength = points.GetLength (0);
		int yLength = points.GetLength (1);
		int zLength = points.GetLength (2);
		float[,] heightmap = TerrainGenerator.GetTerrain (xLength * yLength, zLength);
		for (int y = 0; y < yLength; y ++) {
			for (int x = 0; x < xLength; x ++) {
				for (int z = 0; z < zLength; z ++) {
					GridPoint gp = Instantiate (gridPoint, points[x, y, z], Quaternion.identity) as GridPoint;
					gp.transform.position = new Vector3 (
						gp.transform.position.x,
						gp.transform.position.y + heightmap[(y * xLength) + x, z] * 50f,
						gp.transform.position.z
					);
					gp.Create (Vector3.zero, Vector3.zero);
				}
			}
		}
	}
	
	void Update () {
		helix.DrawHelix ();	
	}
}

