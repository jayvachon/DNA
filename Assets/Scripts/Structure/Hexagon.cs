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

	void Awake () {
		sideCount = 6;
		sideLength = Structure.scale;
		circumradius = sideCount * sideLength;
		float yellow = Random.Range (0.75f, 1f);
		Color c = new Color (yellow, yellow, 0f);
		Init (CustomMesh.Hexagon (), c, true);
	}
}
