using UnityEngine;
using System.Collections;
using Pathing;
using Units;

public class PathRotator : MBRefs {

	public override Vector3 Position {
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
	}

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

	float pathProgress = 0;
	float PathProgress {
		get { 
			if (Path == null) return 0;
			return Path.pathPositioner.Progress; 
		}
	}

	float maxDistance = 1f;
	float MobileDistance {
		set {
			float mobileDistance = TrigMap.HalfCos (value);
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
