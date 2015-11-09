using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DNA.Paths;

namespace DNA {

	public delegate void OnLoadFog ();

	public class FogOfWarManager : MBRefs {

		public OnLoadFog OnLoadFog { get; set; }

		public void Init () {
			StartCoroutine (CreateFog ());
		}

		IEnumerator CreateFog () {

			const int blockSize = 100;

			List<GridPoint> gpoints = TreeGrid.Points;
			int pointCount = gpoints.Count;

			for (int i = 0; i < pointCount; i ++) {

				FogOfWar f = ObjectPool.Instantiate<FogOfWar> ();
				f.Point = gpoints[i];
				f.Parent = MyTransform;
				f.Position = f.Point.Position;
				/*float scale = Mathf.Max (0.5f, Mathf.Pow (i, 1.0000001f) * 0.02f);
				f.transform.localScale = new Vector3 (
					f.transform.localScale.x * scale,
					f.transform.localScale.y * scale,
					f.transform.localScale.z * scale
				);*/
				LookAtCenter (f.MyTransform);

				if (i % blockSize == 0)
					yield return null;
			}

			if (OnLoadFog != null)
				OnLoadFog ();
		}

		void LookAtCenter (Transform t) {
			float y = -75;
			t.LookAt (new Vector3 (0, y, 0), Vector3.up);
			t.SetLocalEulerAnglesX (t.localEulerAngles.x - 90f);
		}
	}
}