using UnityEngine;
using System.Collections;

// Generates points arranged along Fermat's spiral

public class Helix {

	public readonly Vector3 center 	= Vector3.zero;
	public readonly int pointCount 	= 200;
	public readonly float maxRadius	= 3f; // distance from center to side
	public readonly float altitude	= 0.165f;  // distance between a side and the side one rotation above or below it
	public readonly float angle		= 137.508f;

	float bottomDiameter = 0;
	float parabolaHeight = 0;

	Vector4[] points;
	public Vector4[] Points {
		get { return points; }
	}

	Vector3[] positions;
	public Vector3[] Positions {
		get { return positions; }
	}

	Vector3[] rotations;
	public Vector3[] Rotations {
		get { return rotations; }
	}

	Vector3 LastPoint {
		get {
			Vector4 last = Points[points.Length-1];
			return new Vector3 (0, last.y, last.z);
		}
	}

	Vector3 LastPosition {
		get { return positions[positions.Length-1]; }
	}

	public Helix (int pointCount=200) {
		this.pointCount = pointCount;
		CreateHelix ();
	}

	void CreateHelix () {
		
		//points = new Vector4[sides * rotations];
		positions = new Vector3[pointCount];
		rotations = new Vector3[pointCount];
		float radius = maxRadius;

		for (int i = 0; i < positions.Length; i ++) {
			float deg = (float)i * angle;
			float radians = deg * Mathf.Deg2Rad;
			radius = maxRadius * Mathf.Sqrt (radians);
			positions[i] = new Vector3 (
				center.x + radius * Mathf.Sin (radians),
				center.y + i * -altitude,
				center.z + radius * Mathf.Cos (radians)
			);
			/*points[i] = new Vector4 (
				center.x + radius * Mathf.Sin (radians),
				center.y + i * -altitude,
				center.z + radius * Mathf.Cos (radians),
				deg
			);*/
		}

		bottomDiameter = radius * 2;
		parabolaHeight = positions.Length * altitude;

		float focus = -16.3615f; // ~ but why???
		float directrix = -focus;
		Vector3 v3focus = new Vector3 (0, focus, 0);

		for (int i = 0; i < rotations.Length; i ++) {
			Vector3 position = positions[i];
			Vector3 v3directrix = new Vector3 (position.x, directrix, position.z);
			float deg = (float)i * angle;
			float distToDirectrix = directrix - position.y;
			float distToFocus = Vector3.Distance (position, v3focus);
			float focusDirectrixDist = Vector3.Distance (v3focus, v3directrix);
			float pythag = Mathf.Pow (distToDirectrix, 2) + 
				Mathf.Pow (distToFocus, 2) - 
				Mathf.Pow (focusDirectrixDist, 2);
			if (i == 10) {
				Debug.Log (distToFocus);
				Debug.Log (Mathf.Acos ((pythag / (2 * distToDirectrix * focusDirectrixDist)) * Mathf.Deg2Rad));
			}
			rotations[i] = new Vector3 (
				0, //Mathf.Acos ((pythag / (2 * distToDirectrix * focusDirectrixDist)) * Mathf.Deg2Rad) * Mathf.Rad2Deg,
				deg,
				0
			);
		}
	}

	int Fibonacci (int n) {
		if (n <= 0)
			return 0;
		if (n == 1)
			return 1;
		return Fibonacci (n-1) + Fibonacci (n-2);
	}

	public void Draw () {
		int count = 60;
		float height = parabolaHeight;
		Vector3 start = LastPosition;
		Vector3 end = new Vector3 (0, LastPosition.y, LastPosition.z-bottomDiameter);
		Vector3 lastPoint = start;
		float focus = bottomDiameter/parabolaHeight*4;
		float directrix = -focus;
		for (int i = 0; i < count+1; i ++) {
			Vector3 point = SampleParabola (start, end, height, (float)i / (float)count);
			Debug.DrawLine (lastPoint, point);
			lastPoint = point;
		}
		/*int skip = 5;
		Color[] colors = new Color[] {Color.red, Color.yellow, Color.white, Color.black, Color.green};
		for (int j = 0; j < skip; j ++) {
			for (int i = j; i < points.Length - 1; i += skip) {
				if (i + skip < points.Length)
					Debug.DrawLine (points[i], points[i + skip], colors[j]);
			}
		}*/
	}

	Vector3 SampleParabola ( Vector3 start, Vector3 end, float height, float t ) {
        float parabolicT = t * 2 - 1;
        if ( Mathf.Abs( start.y - end.y ) < 0.1f ) {
            //start and end are roughly level, pretend they are - simpler solution with less steps
            Vector3 travelDirection = end - start;
            Vector3 result = start + t * travelDirection;
            result.y += ( -parabolicT * parabolicT + 1 ) * height;
            return result;
        } else {
            //start and end are not level, gets more complicated
            Vector3 travelDirection = end - start;
            Vector3 levelDirecteion = end - new Vector3( start.x, end.y, start.z );
            Vector3 right = Vector3.Cross( travelDirection, levelDirecteion );
            Vector3 up = Vector3.Cross( right, travelDirection );
            if ( end.y > start.y ) up = -up;
            Vector3 result = start + t * travelDirection;
            result += ( ( -parabolicT * parabolicT + 1 ) * height ) * up.normalized;
            return result;
        }
    }
}