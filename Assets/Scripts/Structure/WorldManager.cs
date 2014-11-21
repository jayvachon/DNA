using UnityEngine;
using System.Collections;

public class WorldManager : MonoBehaviour {

	public Ring ring;
	public GridPoint gridPoint;
	public CustomMeshObject cmo;

	int ringSideCount = 12;

	Helix helix;
	Helix helix2;
	TriangleGrid triGrid;
	TerrainGenerator terrainGenerator;
	float[,] heightmap;

	void Start () {

		helix2 = new Helix (Vector3.zero, 2, 30, 360, 720);
		Ring[] rings = CreateRingsOnHelix ();

		Vector3[] vs = new Vector3[rings.Length * (ringSideCount*2) * 3];
		int count = 0;
		for (int i = 0; i < rings.Length-1; i ++) {

			Vector3[] points = rings[i].GetRingPoints ();
			Vector3[] points2 = rings[i+1].GetRingPoints ();

			for (int j = 0; j < ringSideCount-1; j ++) {

				vs[count] = points2[j];
				vs[count+1] = points[j+1];
				vs[count+2] = points[j];

				vs[count+3] = points2[j];
				vs[count+4] = points2[j+1];
				vs[count+5] = points[j+1];

				count += 6;
			}

			vs[count] = points2[ringSideCount-1];
			vs[count+1] = points[0];
			vs[count+2] = points[ringSideCount-1];

			vs[count+3] = points2[ringSideCount-1];
			vs[count+4] = points2[0];
			vs[count+5] = points[0];

			count += 6;
		}

		Mesh helixTerrain = CustomMesh.CreateMesh (vs);

		CustomMeshObject c = Instantiate (cmo, Vector3.zero, Quaternion.identity) as CustomMeshObject;
		c.Init (helixTerrain, Color.green, true);

		CustomMeshObject c2 = Instantiate (cmo, Vector3.zero, Quaternion.identity) as CustomMeshObject;
		c2.Init (helixTerrain, Color.white, true);
		c2.transform.SetLocalEulerAnglesY (180f);
	}

	void DeprecatedStart () {

		helix = new Helix (Vector3.zero, 2, 60, 240, 720);
		triGrid = new TriangleGrid ();
		
		Vector3[,,] points = triGrid.CreateGridOnHelix (helix, 13);
		int xLength = points.GetLength (0);
		int yLength = points.GetLength (1);
		int zLength = points.GetLength (2);
		
		Vector3[,,] positions = new Vector3[xLength, yLength, zLength];
		InitTerrain (xLength, yLength, zLength);
		
		for (int y = 0; y < yLength; y ++) {
			for (int x = 0; x < xLength; x ++) {
				for (int z = 0; z < zLength; z ++) {
					
					float elevation = SetElevation ((y * xLength) + x, z);
					Vector3 position = points[x, y, z];
					
					positions[x, y, z] = new Vector3 (
						position.x,
						position.y + elevation,
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

	void InitTerrain (int xLength, int yLength, int zLength) {
		terrainGenerator = new TerrainGenerator ();
		heightmap = terrainGenerator.GetFlatTerrain (xLength * yLength, zLength);
	}

	float SetElevation (int x, int y) {
		float height = heightmap[x, y];
		float elevation = height * 60f;
		return elevation;
	}

	Ring[] CreateRingsOnHelix () {
		Vector3[] points = helix2.points;
		Ring[] rings = new Ring[points.Length];
		for (int i = 0; i < points.Length; i ++) {
			rings[i] = Instantiate (ring, points[i], Quaternion.identity) as Ring;
			rings[i].Create (i % 2 == 0, ringSideCount);
			rings[i].transform.SetLocalEulerAnglesY (helix2.pointRotations[i]);
		}
		return rings;
	}
}

