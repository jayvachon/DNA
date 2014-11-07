using System;
using UnityEngine;
using System.Collections;

public class Hexagon2 : CustomMeshObject {
	
	float sideLength = 10f;
	public float SideLength {
		get { return sideLength; }
	}
	
	float sideCount = 6f;
	public float SideCount {
		get { return sideCount; }
	}
	
	float circumradius = 600f;
	public float Circumradius {
		get { return circumradius; }
	}

	float apothem = 0f;
	public float Apothem {
		get { return apothem; }
	}

	Vector3 coordinates;
	public Vector3 Coordinates {
		get { return coordinates; }
		set { coordinates = value; }
	}

	Vector3 point;
	public Vector3 Point {
		get { return point; }
		set { point = value; }
	}

	float heightInterval = 10f;
	public float HeightInterval {
		get { return heightInterval; }
	}

	float height;
	public float Height {
		get { return height; }
	}

	public enum Tile {
		Milk,
		Dirt,
		Grass
	}
	Tile hexTile = Tile.Dirt;
	public Tile HexTile {
		get { return hexTile; }
	}

	bool flat = false;
	WorldManager wm;

	void Awake () {
		sideCount = 6;
		sideLength = Structure.scale;
		circumradius = sideCount * sideLength;
		apothem = SideLength / (2f * Mathf.Tan (Mathf.PI / SideCount));
		SetType (0.5f);
	}

	public void Init (Vector3 _coordinates, Vector3 _point, float noiseVal, WorldManager _wm) {
		Coordinates = _coordinates;
		Point = _point;
		wm = _wm;
		SetType (noiseVal);
	}

	public void UpdateHeights () {
		StretchToNeighbors ();
	}

	void SetType (float noiseVal) {
		int typeCount = Enum.GetNames (typeof (Tile)).Length;
		noiseVal = Mathf.Clamp (
			Mathf.Floor (noiseVal * (float)typeCount + 1f) - 1,
			0,
			typeCount - 1
		);
		SetType ((Tile)noiseVal);
	}
	
	void SetType (Tile t) {
		hexTile = t;
		switch (hexTile) {
			case Tile.Milk: InitMilk (); break;
			case Tile.Dirt: InitDirt (); break;
			case Tile.Grass: InitGrass (); break;
		}
	}
	
	void InitDirt () {
		float yellow = UnityEngine.Random.Range (0.75f, 1f);
		Color c = new Color (yellow, yellow, 0f);
		Init (CustomMesh.Hexagon (), c, true);
	}
	
	void InitGrass () {
		flat = true;
		Color c = new Color (
			0,
			UnityEngine.Random.Range (0.75f, 1f),
			0
		);
		Init (CustomMesh.Hexagon (), c, true);
	}
	
	void InitMilk () {
		flat = true;
		Init (CustomMesh.Hexagon (), Color.white, true);
	}

	void StretchToNeighbors () {
//		if (hexTile == Tile.Milk) return;
		float[] heights = new float[6];
		Vector3[] neighbors = new Vector3[6];
		neighbors[0] = new Vector3 (Coordinates.x, 0, Coordinates.z + 1);
		neighbors[1] = new Vector3 (Coordinates.x + 1, 0, Coordinates.z + 1);
		neighbors[2] = new Vector3 (Coordinates.x, 0, Coordinates.z - 1);
		neighbors[3] = new Vector3 (Coordinates.x - 1, 0, Coordinates.z - 1);
		neighbors[4] = new Vector3 (Coordinates.x - 1, 0, Coordinates.z);
		neighbors[5] = new Vector3 (Coordinates.x - 1, 0, Coordinates.z + 1);
		for (int i = 0; i < neighbors.Length; i ++) {
			Hexagon2 h = wm.GetHexagonAtCoordinate(neighbors[i]);
			if (h == null) {
				heights[i] = Point.y;
			} else {
				heights[i] = h.Point.y - Point.y;
			}
			heights[i] /= heightInterval;
			//heights[i] *= 0.5f;
			if (Coordinates.x == 1 && Coordinates.z == 1) {
				if (h == null) {
					Debug.Log (i + ": " + heights[i] + " at null");
				} else {
					Debug.Log (i + ": " + heights[i] + " at " + h.Coordinates.x + ", " + h.Coordinates.z);
				}
			}
		}
		Init (CustomMesh.Hexagon (heights), Color.green, false);
	}
}
