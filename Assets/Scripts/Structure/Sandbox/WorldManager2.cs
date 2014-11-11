using UnityEngine;
using System.Collections;

public class WorldManager2 : MonoBehaviour {

	public GameObject cube;
	public CustomMeshObject cmo;

	HexGridCreator hexGridCreator;
	HelixCreator helixCreator;

	void Awake () {
		hexGridCreator = new HexGridCreator (10f, 2);
		Vector2 center = hexGridCreator.GetCenter ();
		helixCreator = new HelixCreator (new Vector3 (center.x, 0, center.y));
		InstantiateCubes (hexGridCreator.XSize, hexGridCreator.YSize, hexGridCreator.Positions);
	}

	void InstantiateCubes (int xSize, int ySize, Vector3[,] positions) {
		for (int x = 0; x < xSize; x ++) {
			for (int y = 0; y < ySize; y ++) {
				Vector3 p = positions[x, y];
				p.y = helixCreator.PointOnHelix (p, 30f);
				if (p.y > -1f) {
					Instantiate (cube, p, Quaternion.identity);
				}
			}
		}
	}

	/*void CreateHexagon () {
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
	}*/

	void Update () {
		helixCreator.DrawHelix ();
	}
}

public class HexGridCreator {

	float cellSize = 10f;
	float heightInterval = 100f;

	int xSize = 50;
	public int XSize {
		get { return xSize; }
	}

	int ySize = 50;
	public int YSize {
		get { return ySize; }
	}

	Vector3[,] coords;
	public Vector3[,] Coords {
		get { return coords; }
	}

	Vector3[,] positions;
	public Vector3[,] Positions {
		get { return positions; }
	}

	int width;

	public HexGridCreator (float cellSize, int width) {
		this.cellSize = cellSize;
		this.width = width;
		CreateGrid ();
//		SetHeight ();
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

	public Vector2 GetCenter () {
		return new Vector2 (xSize * cellSize / 2, ySize * cellSize / 2);
	}
}


public class HelixCreator {

	Vector3 center = new Vector3 (100, 0, 105);
	int rotations = 10;
	int radius = 100;
	int rise = 100;
	int sideCount = 6;
	float sideLength;
	Vector3[] points;

	public HelixCreator (Vector3 center, int rotations = 10, int radius = 200, int rise = 100, int sideCount = 6) {
		this.center = center;
		this.rotations = rotations;
		this.radius = radius;
		this.rise = rise;
		this.sideCount = sideCount;
		points = new Vector3[sideCount * rotations];
		CreateHelix ();
		SetSideLength ();
	}

	void CreateHelix () {
		float pointRise = (float)rise / (float)sideCount;
		float deg = 360f / (float)sideCount;
		for (int i = 0; i < points.Length; i ++) {
			float radians = (float)i * deg * Mathf.Deg2Rad;
			points[i] = new Vector3 (
				center.x + radius * Mathf.Sin (radians),
				center.y + i * pointRise,
				center.z + radius * Mathf.Cos (radians)
			);
		}
	}

	void SetSideLength () {
		sideLength = Vector2.Distance (new Vector2 (points[0].x, points[0].z), new Vector2 (points[1].x, points[1].z));
	}

	bool PointOnLineSegment (Vector3 pt1, Vector3 pt2, Vector3 pt, float epsilon = 10f) {
		if (pt.x - Mathf.Max (pt1.x, pt2.x) >= epsilon || 
		    Mathf.Min (pt1.x, pt2.x) - pt.x >= epsilon || 
		    pt.z - Mathf.Max (pt1.z, pt2.z) >= epsilon ||
		    Mathf.Min (pt1.z, pt2.z) - pt.z >= epsilon)
			return false;

		if (Mathf.Abs (pt2.x - pt1.x) < epsilon)
			return Mathf.Abs (pt1.x - pt.x) < epsilon || Mathf.Abs (pt2.x - pt.x) < epsilon;
		if (Mathf.Abs (pt2.z - pt1.z) < epsilon)
			return Mathf.Abs (pt1.z - pt.z) < epsilon || Mathf.Abs(pt2.z - pt.z) < epsilon;

		float x = pt1.x + (pt.z - pt1.z) * (pt2.x - pt1.x) / (pt2.z - pt1.z);
		float z = pt1.z + (pt.x - pt1.x) * (pt2.z - pt1.z) / (pt2.x - pt1.x);
		
		return Mathf.Abs (pt.x - x) < epsilon || Mathf.Abs (pt.z - z) < epsilon;
	}

	float GetYOnLineSegment (Vector3 pt1, Vector3 pt2, Vector3 pt) {
		float dis = Vector2.Distance (new Vector2 (pt1.x, pt1.z), new Vector2 (pt.x, pt.z));
		return pt1.y + (pt2.y - pt1.y) * (dis / sideLength);
	}
	
	public float PointOnHelix (Vector3 pt, float epsilon = 50f) {
		for (int i = 0; i < points.Length - 1; i ++) {
			if (PointOnLineSegment (points[i], points[i + 1], pt, epsilon))
				return GetYOnLineSegment (points[i], points[i + 1], pt);
		}
		return -1f;
	}
	
	public void DrawHelix () {
		for (int i = 0; i < points.Length - 1; i ++) {
			Debug.DrawLine (points[i], points[i + 1], Color.red);
		}
	}
}