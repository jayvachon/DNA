using UnityEngine;
using System.Collections;

public class Hexagon : CustomMeshObject {

	private float sideLength = 100f;
	public float SideLength {
		get { return sideLength; }
	}

	private float sideCount = 6f;
	public float SideCount {
		get { return sideCount; }
	}

	private float circumradius = 600f;
	public float Circumradius {
		get { return circumradius; }
	}

	private void Awake () {
		sideCount = 6;
		sideLength = Structure.scale;
		circumradius = sideCount * sideLength;
		float yellow = Random.Range (0.75f, 1f);
		Color c = new Color (yellow, yellow, 0f);
		Init (CustomMesh.Hexagon (), c, true);
	}
}
