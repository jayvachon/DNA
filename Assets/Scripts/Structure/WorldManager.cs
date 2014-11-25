using UnityEngine;
using System.Collections;

public class WorldManager : MonoBehaviour {

	public Ring ring;
	public GridPoint gridPoint;
	public CustomMeshObject cmo;

	int ringSideCount = 16;

	Helix helix;
	TerrainGenerator terrainGenerator;
	float[,] heightmap;

	int helixWidth = 540;
	int rotationHeight = 960;
	int minRingWidth = 100;
	float maxRingWidth = 200f;

	void Awake () {
		helix = new Helix (Vector3.zero, 2, 45, helixWidth, rotationHeight);
		InitTerrain (1, 1, helix.points.Length, 20f);
	}

	void Start () {

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

	void InitTerrain (int xLength, int yLength, int zLength, float scale) {
		terrainGenerator = new TerrainGenerator ();
		heightmap = terrainGenerator.GetTerrain (xLength * yLength, zLength, scale);
	}

	Ring[] CreateRingsOnHelix () {

		Vector3[] points = helix.points;
		int ringsCount = points.Length;
		Ring[] rings = new Ring[ringsCount];

		for (int i = 0; i < ringsCount; i ++) {

			int width = minRingWidth + (int)(heightmap[0, i] * maxRingWidth);
			bool offset = i % 2 == 0;
			offset = false; // uncomment for smooth terrain

			rings[i] = Instantiate (ring, points[i], Quaternion.identity) as Ring;
			rings[i].Create (offset, ringSideCount, width);
			rings[i].transform.SetLocalEulerAnglesY (helix.pointRotations[i]);
		}
		return rings;
	}
}

