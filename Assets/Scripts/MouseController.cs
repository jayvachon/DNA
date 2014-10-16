using UnityEngine;
using System.Collections;

public class MouseController : MonoBehaviour {

	private float maxDistance = 5000f;

	void FixedUpdate () {
		if (Input.GetMouseButtonDown (0)) {
			Click ();
		}
	}

	private void Click () {
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		RaycastHit hit;
		if (Physics.Raycast (ray, out hit, maxDistance)) {
			Events.instance.Raise (new MouseClickEvent (hit));
		}
	}

	void Update () {
		if (Input.GetKeyDown (KeyCode.Space)) {
			float[,] terrain = TerrainGenerator.GetRoundedTerrain (24, 3);
			for (int x = 0; x < 25; x ++) {
				for (int y = 0; y < 24; y ++) {
					Debug.Log (terrain[x, y]);
				}
			}
		}
	}
}
