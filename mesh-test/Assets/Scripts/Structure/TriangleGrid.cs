using UnityEngine;
using System.Collections;

public class TriangleGrid {

	double triSize = 10;
	Vector3[,,] coords;
	Vector3[,,] positions;
	int width = 9;

	public Vector3[,,] Coords {
		get { return coords; }
	}

	public TriangleGrid () {
	}

	public float GetGridWidth () {
		return Vector3.Distance (positions[0, 0, 0], positions[0, 0, width-1]);
	}

	public Vector3[,,] CreateGridOnHelix (Helix h, int w = 9) {

		width = w;

		Vector3[] points = h.points;
		triSize = h.sideLength;

		int length = h.sideCount;	// along the line of the helix
		int height = h.rotations;	// vertically up the helix
		int center = width / 2;		// the z coord that falls on the line

		coords = new Vector3[length, height, width];
		for (int y = 0; y < height; y ++) {
			for (int x = 0; x < length; x ++) {
				for (int z = 0; z < width; z ++) {
					coords[x, y, z] = new Vector3 (x, y, z);
				}
			}
		}

		positions = new Vector3[length, height, width];
		for (int y = 0; y < height; y ++) {
			for (int x = 0; x < length; x ++) {

				// get the angle from this point on the line to the center of the helix
				Vector3 targetDir = h.center - Vector3.Normalize (new Vector3 (points[x].x, 0, points[x].z));
				float angle = Vector3.Angle (targetDir, Vector3.back);
				
				// compensate for the fact that Vector3.Angle only returns acute angles
				if (x > length / 2) {
					angle = Mathf.Abs (angle - 180);
					angle += 180;
				}
				angle *= Mathf.Deg2Rad;

				for (int z = 0; z < width; z ++) {

					// get the distance from the point, perpendicular to the line of the helix
					float dis = (float)triSize * (z - center);
					if (x % 2 == 0) {
						dis += (float)triSize * 0.5f;
					}

					// calculate the position
					float xPos = points[x].x + dis * Mathf.Sin (angle);
					float yPos = points[(y * length) + x].y;
					float zPos = points[x].z + dis * Mathf.Cos (angle);
					positions[x, y, z] = new Vector3 (xPos, yPos, zPos);
				}
			}
		}

		return positions;
	}
}
