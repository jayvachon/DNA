using UnityEngine;
using System.Collections;

namespace DNA {

	public class Levee : MonoBehaviour {

		void Awake () {
			Init ();
		}

		void Init () {
			int count = 5;
			float radius = 70;
			float deg = 360 / (float)count;
			for (int i = 0; i < count; i ++) {
				float radians = deg * i * Mathf.Deg2Rad;
				Transform t = ObjectPool.Instantiate<LeveeWall> ().transform;
				t.position = new Vector3 (
					Mathf.Sin (radians) * radius,
					0,
					Mathf.Cos (radians) * radius
				);
				t.localEulerAngles = new Vector3 (
					t.localEulerAngles.x,
					deg * i,
					t.localEulerAngles.z
				);
				t.SetParent (transform);
				t.SetLocalPositionY (0.5f);
				t.SetLocalScaleY (1f);
			}
		}
	}
}