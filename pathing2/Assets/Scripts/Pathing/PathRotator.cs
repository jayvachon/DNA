using UnityEngine;
using System.Collections;

namespace Pathing {

	public class PathRotator : MBRefs {

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
					IPathable pathable = MobileTransform.GetScript<IPathable> ();
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

		public float Distance { get; private set; }

		Vector3[] line;

		public void StartMoving () {
			if (line != null && !SameLine (Positioner.Line)) return;
			line = Positioner.Line;
			#if UNITY_EDITOR
			if (line.Length > 2)
				Debug.LogWarning ("Path should only have 2 points");
			#endif
			SetPosition ();
			SetRotation ();
			Distance = Vector3.Distance (line[0], line[1]);
		}

		void SetPosition () {
			Vector3 a = line[0];
			Vector3 b = line[1];
			MyTransform.position = new Vector3 (
				(a.x + b.x) / 2,
				0,
				(a.z + b.z) / 2
			);
		}

		void SetRotation () {
			Vector3 c = line[0] - line[1];
			float angle = Vector3.Angle (c, Vector3.right);
			Vector3 cross = Vector3.Cross (c, Vector3.right);
			angle *= -Mathf.Sign (cross.y);
			transform.SetLocalEulerAnglesY (angle);
		}

		bool SameLine (Vector3[] otherLine) {
			bool same1 = line[0] == otherLine[0] && line[1] == otherLine[1];
			//bool same2 = line[0] == otherLine[1] && line[1] == otherLine[0];
			return same1;// || same2;
		}
	}
}