using UnityEngine;
using System.Collections;

public class WorldManager : MonoBehaviour {

	public GridPoint gridPoint;
	public CustomMeshObject cmo;

	Helix helix;
	TriangleGrid triGrid;
	TerrainGenerator terrainGenerator;

	void Start () {
	
		terrainGenerator = new TerrainGenerator ();
		helix = new Helix (Vector3.zero, 2, 60, 240, 720);
		triGrid = new TriangleGrid ();

		Vector3[,,] points = triGrid.CreateGridOnHelix (helix, 13);
		int xLength = points.GetLength (0);
		int yLength = points.GetLength (1);
		int zLength = points.GetLength (2);
		Vector3[,,] positions = new Vector3[xLength, yLength, zLength];
		float[,] heightmap = terrainGenerator.GetTerrain (xLength * yLength, zLength);

		float halfDeg = 180f / (float)zLength;
		float deg = 360f / (float)zLength;

		float width = (triGrid.GetGridWidth() / Mathf.PI);

		for (int y = 0; y < yLength; y ++) {
			for (int x = 0; x < xLength; x ++) {
				float zCenter = points[x, y, zLength / 2].z;
				for (int z = 0; z < zLength; z ++) {
					//float radians = ((float)z * halfDeg + 180f) * Mathf.Deg2Rad;
					//float lift = Mathf.Sin (radians) * 60f;
					//float zOff = Mathf.Cos (radians) * 60f;
					float radians = ((float)z * deg) * Mathf.Deg2Rad;
					float lift = Mathf.Cos (radians) * 30f;
					float elevation = heightmap[(y * xLength) + x, z] * 60f;
					Vector3 position = points[x, y, z];
					/*GridPoint gp = Instantiate (gridPoint, position, Quaternion.identity) as GridPoint;
					gp.transform.position = new Vector3 (
						gp.transform.position.x,
						gp.transform.position.y + elevation + lift,
						gp.transform.position.z
					);
					gp.Create (triGrid.Coords[x, y, z], position);
					positions[x, y, z] = gp.transform.position;*/
					positions[x, y, z] = new Vector3 (
						position.x,
						position.y + elevation + lift,
						position.z
					);
				}
			}
		}

		Vector3[] tri = new Vector3[6];
		tri[0] = positions[0, 0, 0];
		tri[1] = positions[0, 0, 1];
		tri[2] = positions[1, 0, 1];

		tri[3] = positions[1, 0, 0];
		tri[4] = positions[0, 0, 0];
		tri[5] = positions[1, 0, 1];

		Vector3[] vs = new Vector3[xLength * yLength * zLength * 6];
		int count = 0;
		for (int y = 0; y < yLength; y ++) {
			for (int x = 0; x < xLength; x ++) {
				if (x > xLength - 1) continue;
				for (int z = 0; z < zLength; z ++) {
					if (z > zLength - 2) continue;

					// this bridges the gap between rotations
					if (x > xLength - 2) {
						if (y >= yLength - 1)
							continue;
						vs[count] = positions[x, y, z];
						vs[count+1] = positions[x, y, z+1];
						vs[count+2] = positions[0, y+1, z+1];
						
						vs[count+3] = positions[0, y+1, z];
						vs[count+4] = positions[x, y, z];
						vs[count+5] = positions[0, y+1, z+1];
						count += 6;
						continue;
					}

					vs[count] = positions[x, y, z];
					vs[count+1] = positions[x, y, z+1];
					vs[count+2] = positions[x+1, y, z+1];

					vs[count+3] = positions[x+1, y, z];
					vs[count+4] = positions[x, y, z];
					vs[count+5] = positions[x+1, y, z+1];
					count += 6;
				}
			}
		}

		Mesh helixTerrain = CustomMesh.CreateMesh (vs);

		CustomMeshObject c = Instantiate (cmo, Vector3.zero, Quaternion.identity) as CustomMeshObject;
		c.Init (helixTerrain, Color.green, true);

		CustomMeshObject c2 = Instantiate (cmo, Vector3.zero, Quaternion.identity) as CustomMeshObject;
		c2.Init (helixTerrain, Color.white, true);
		c2.transform.SetLocalEulerAnglesY (180f);
	}
}

