#define DEBUG
using UnityEngine;
using System.Collections;

namespace DNA {

	public class OuterSea : Sea2 {

		public Levee levee;
		float riseCoefficient = 
		#if DEBUG
		0.001f
		#else
		0.00001f
		#endif
		;

		protected override void Awake () {
			base.Awake ();
			Init ();
		}

		void Init () {
			RegularPolygon edges = levee.edges;
			for (int i = 0; i < edges.SideCount; i ++) {
				OuterSeaPiece sea = ObjectPool.Instantiate<OuterSeaPiece> (
					edges.Positions[i],
					new Vector3 (90, 0, 0).ToQuaternion ()
				);
				sea.Parent = MyTransform;
				sea.transform.SetLocalPositionY (0f);
				edges.ApplyAngleY (sea.MyTransform, i);
			}
		}

		void OnEnable () {
			EmissionsManager.onUpdate += OnUpdateEmissions;
		}

		void OnDisable () {
			EmissionsManager.onUpdate -= OnUpdateEmissions;
		}

		void OnUpdateEmissions (float val) {
			riseRate = val;
		}

		void Update () {
			Level += riseRate * riseCoefficient;
			#if DEBUG
			if (Input.GetKeyDown (KeyCode.Z)) {
				riseRate += 1f;
			}
			if (Input.GetKeyDown (KeyCode.X)) {
				riseRate -= 1f;
			}
			#endif
		}
	}
}