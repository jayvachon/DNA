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

	enum Type {
		Dirt, 
		Grass,
		Milk
	}
	Type type = Type.Dirt;

	void Awake () {
		sideCount = 6;
		sideLength = Structure.scale;
		circumradius = sideCount * sideLength;
		//SetType ();
	}

	public void SetType (float noiseVal) {
		int typeCount = Enum.GetNames (typeof (Type)).Length;
		noiseVal = Mathf.Clamp (
			Mathf.Floor (noiseVal * (float)typeCount + 1f) - 1,
			0,
			typeCount - 1
		);
		SetType ((Type)noiseVal);
	}

	void SetType (Type t) {
		//int t = UnityEngine.Random.Range (0, Enum.GetNames (typeof (Type)).Length);
		//type = (Type)t;
		type = t;
		switch (type) {
			case Type.Dirt: InitDirt (); break;
			case Type.Grass: InitGrass (); break;
			case Type.Milk: InitMilk (); break;
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
