using UnityEngine;
using System.Collections;

namespace DNA {

	public class Levee : MBRefs {

		const float WallHeight = 1f;
		const float WallWidth = 2f;

		public float Height {
			get { return MyTransform.localScale.y; }
			set { MyTransform.SetLocalScaleY (value); }
		}

		public readonly RegularPolygon edges = new RegularPolygon (5, 75f);
		readonly float startHeight = 1f;

		void Awake () {
			Height = startHeight;
			Init ();
		}

		void Init () {

			for (int i = 0; i < edges.SideCount; i ++) {
				Transform t = ObjectPool.Instantiate<LeveeWall> ().transform;
				t.position = edges.Positions[i];
				edges.ApplyAngleY (t, i);
				t.SetParent (transform);
				t.SetLocalPositionY (0.5f);
				t.localScale = new Vector3 (
					edges.SideLength + 1.25f,
					WallHeight,
					WallWidth
				);
			}
		}
	}
}