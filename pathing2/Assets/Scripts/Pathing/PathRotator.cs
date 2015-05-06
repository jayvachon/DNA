using UnityEngine;
using System.Collections;
using Pathing;
using Units;

public class PathRotator : MBRefs {

	float progress;
	public float Progress {
		get { return progress; }
		set {
			
			if (prevProgress > value) {
				progressOffset = progressOffset == 0 ? 1 : 0;
			}
			prevProgress = value;
			float totalProgress = (progressOffset + value) / 2f;
			MobileDistance = totalProgress;
			MyTransform.position = Positioner.PathPosition;
			Rotation = totalProgress;
		}
	}

	float pathLength = 0;
	float PathLength {
		get { return pathLength; }
		set {
			pathLength = value;
			pointLength = 1 - (pathLength - radius) / pathLength;
		}
	}

	float pointLength = 0;

	Transform mobileTransform;
	Transform MobileTransform {
		get {
			if (mobileTransform == null) {
				mobileTransform = MyTransform.GetChild (0);
			}
			return mobileTransform;
		}
	}

	Path path;
	Path Path {
		get {
			if (path == null) {
				IPathable pathable = MobileTransform.GetScript<MobileUnitTransform> () as IPathable;
				path = pathable.Path;
			}
			return path;
		}
	}

	PathPositioner positioner;
	PathPositioner Positioner {
		get {
			if (positioner == null) {
				positioner = Path.Positioner;
			}
			return positioner;
		}
	}

	float radius = 1f;
	float MobileDistance {
		set {
			float d = value <= 0.5f
				? Mathf.Clamp (Mathf.Lerp (-pointLength, 0.5f + pointLength, value*2), 0, 0.5f)
				: Mathf.Clamp (Mathf.Lerp (0.5f - pointLength, 1f + pointLength, (value*2)-1), 0.5f, 1f);

			float mobileDistance = TrigMap.HalfCos (d);
			MobileTransform.SetLocalPositionX (mobileDistance * radius);
		}
	}

	float Rotation {
		set {
			float p = TrigMap.Sin (value);
			if (value >= 0.5f && value < 1.5f) {
				MyTransform.SetLocalEulerAnglesY (angleOffset + Mathf.Lerp (-25f, 205f, Mathf.InverseLerp (-1, 1, p)));
			} else {
				MyTransform.SetLocalEulerAnglesY (angleOffset + Mathf.Lerp (205f, -25f, Mathf.InverseLerp (1, -1, p)));
			}
		}
	}
	
	float prevProgress = 0f;
	float progressOffset = 0f;
	Vector3 direction;
	float angleOffset = 0;

	Vector3[] line;

	public void StartMoving () {
		MyTransform.position = MobileTransform.position;
		MobileTransform.localPosition = Vector3.zero;

		if (line == null || !SameLine (Positioner.Line)) {
			line = Positioner.Line;
			Vector3 pt1 = Positioner.Line[0];
			Vector3 pt2 = Positioner.Line[1];
			PathLength = Vector3.Distance (pt1, pt2);
			direction = Vector3.Normalize (pt1 - pt2);
			angleOffset = pt2.z > pt1.z ? Vector3.Angle (Vector3.back, direction) : Vector3.Angle (Vector3.forward, direction);
			MyTransform.SetLocalEulerAnglesY (angleOffset);
		}
	}

	bool SameLine (Vector3[] otherLine) {
		bool same1 = line[0] == otherLine[0] && line[1] == otherLine[1];
		bool same2 = line[0] == otherLine[1] && line[1] == otherLine[0];
		Debug.Log (same1 + ", " + same2);
		return same1 || same2;
	}
}
