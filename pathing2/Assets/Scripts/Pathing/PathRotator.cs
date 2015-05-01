using UnityEngine;
using System.Collections;
using Pathing;
using Units;

public class PathRotator : MBRefs {

	// TODO: Path should know its direction
	// slow down around points to allow for rotation

	public override Vector3 Position {
		get { return MyTransform.position; }
		set {
			if (MobileTransform.localPosition != mobilePosition) {
				MyTransform.position = MobileTransform.position;
				MobileTransform.localPosition = mobilePosition;
				Debug.Log (Position);
				prevPosition = value;
			} else {
				direction = Vector3.Normalize (prevPosition-value);
				// MyTransform.localRotation = Quaternion.LookRotation (direction);
				prevPosition = value;
			}
			// MyTransform.SetLocalEulerAnglesY (PathProgress * 360f);
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

	Vector3 mobilePosition = new Vector3 (0, 0, 1);
	Vector3 direction;
	Vector3 prevPosition;
}
