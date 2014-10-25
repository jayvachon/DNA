using System;
using UnityEngine;
using System.Collections;

public class Hexagon : CustomMeshObject {

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

	public enum HexType {
		Dirt, 
		Grass,
		Milk
	}
	HexType hexType = HexType.Dirt;
	public HexType Type {
		get { return hexType; }
	}

	void Awake () {
		sideCount = 6;
		sideLength = Structure.scale;
		circumradius = sideCount * sideLength;
	}

	public void SetType (float noiseVal) {
		int typeCount = Enum.GetNames (typeof (HexType)).Length;
		noiseVal = Mathf.Clamp (
			Mathf.Floor (noiseVal * (float)typeCount + 1f) - 1,
			0,
			typeCount - 1
		);
		SetType ((HexType)noiseVal);
	}

	void SetType (HexType t) {
		hexType = t;
		switch (hexType) {
			case HexType.Dirt: InitDirt (); break;
			case HexType.Grass: InitGrass (); break;
			case HexType.Milk: InitMilk (); break;
		}
	}

	void InitDirt () {
		float yellow = UnityEngine.Random.Range (0.75f, 1f);
		Color c = new Color (yellow, yellow, 0f);
		Init (CustomMesh.Hexagon (), c, true);
	}

	void InitGrass () {
		Color c = new Color (
			0,
			UnityEngine.Random.Range (0.75f, 1f),
			0
		);
		Init (CustomMesh.Hexagon (), c, true);
	}

	void InitMilk () {
		Init (CustomMesh.Hexagon (), Color.white, true);
	}
}
