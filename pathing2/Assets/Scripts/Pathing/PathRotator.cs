using UnityEngine;
using System.Collections;
using Pathing;
using Units;

public class PathRotator : MBRefs {

	float progress;
	public float Progress {
		get { return progress; }
		set {
			Vector3 newPosition = Positioner.PathPosition;
			if (Vector3.Distance (MobileTransform.localPosition, Vector3.zero) > 0f) {
				MyTransform.position = MobileTransform.position;
				MobileTransform.localPosition = Vector3.zero;
				prevPosition = newPosition;
				Debug.Log (Positioner.Line[0]);
				Debug.Log (Positioner.Line[1]);
				PathLength = Vector3.Distance (Positioner.Line[0], Positioner.Line[1]);
			} else {
				direction = Vector3.Normalize (prevPosition-newPosition);
				prevPosition = newPosition;
			}
			if (prevProgress > value) {
				progressOffset = progressOffset == 0 ? 1 : 0;
			}
			prevProgress = value;
			float totalProgress = (progressOffset + value) / 2f;
			MobileDistance = totalProgress;
			Rotation = totalProgress;
			MyTransform.position = newPosition;
			progress = value;
		}
	}

	float pathLength = 0;
	float PathLength {
		get { return pathLength; }
		set {
			pathLength = value;
			pointLength = (pathLength - maxDistance) / pathLength;
			Debug.Log (pathLength + ", " + pointLength);
		}
	}

	float pointLength = 0;

	/*public override Vector3 Position {
		get { return MyTransform.position; }
		set {
			if (MobileTransform.localPosition != Vector3.zero) {
				MyTransform.position = MobileTransform.position;
				MobileTransform.localPosition = Vector3.zero;
				prevPosition = value;
			} else {
				direction = Vector3.Normalize (prevPosition-value);
				// MyTransform.localRotation = Quaternion.LookRotation (direction);
				prevPosition = value;
			}
			// MyTransform.SetLocalEulerAnglesY (PathProgress * 360f);
			// MobileDistance = PathProgress;

			if (prevProgress > PathProgress) {
				if (progressOffset == 0)
					progressOffset = 1;
				else
					progressOffset = 0;
			}
			prevProgress = PathProgress;
			float totalProgress = (progressOffset + PathProgress) / 2f;
			MobileDistance = totalProgress;
			// MyTransform.SetLocalEulerAnglesY (PathProgress * 360f);	
			Rotation = totalProgress;	
			MyTransform.position = value;
		}
	}*/

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

	float maxDistance = 1f;
	float MobileDistance {
		set {
			float mobileDistance = TrigMap.HalfCos (value);
			// Debug.Log (value);
			MobileTransform.SetLocalPositionX (mobileDistance * maxDistance);
		}
	}

	float Rotation {
		set {
			float p = TrigMap.Sin (value);
			if (value >= 0.5f && value < 1.5f) {
				MyTransform.SetLocalEulerAnglesY (Mathf.Lerp (-25f, 205f, Mathf.InverseLerp (-1, 1, p)));
			} else {
				MyTransform.SetLocalEulerAnglesY (Mathf.Lerp (205f, -25f, Mathf.InverseLerp (1, -1, p)));
			}

			// Debug.Log (p * 360f);
			// MyTransform.SetLocalEulerAnglesY (p * 360f);
			// MyTransform.SetLocalEulerAnglesY (90f + (value * 360f));
		}
	}
	
	float prevProgress = 0f;
	float progressOffset = 0f;
	Vector3 direction;
	Vector3 prevPosition;
}
