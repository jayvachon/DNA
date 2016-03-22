using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DNA.Units;

namespace DNA.Paths {

	public delegate void OnLoadPoints ();

	public class PointsManager : MBRefs {

		readonly List<PointContainer> points = new List<PointContainer> ();

		public List<PointContainer> Points {
			get { return points; }
		}

		public OnLoadPoints OnLoadPoints { get; set; }
		public SeaManager sea;
		List<PointContainer> flooded = new List<PointContainer> ();

		public void Init () {
			StartCoroutine (CreatePoints ());
			InvokeRepeating ("ApplySeaLevel", 1f, 1f);
		}

		public void SetUnitAtIndex<T> (int index) where T : StaticUnit {
			points[index].BeginConstruction<T> ();
			points[index].EndConstruction ();
		}

		public List<Connection> GetConnectionsAtIndex (int index) {
			return points[index].Point.Connections;
		}

		IEnumerator CreatePoints () {
			
			const int blockSize = 100;
			const float perlinScale = 20f;

			List<GridPoint> gpoints = TreeGrid.Points;
			int pointCount = gpoints.Count;

			for (int i = 0; i < pointCount; i ++) {

				PointContainer pc = ObjectPool.Instantiate<PointContainer> ();
				pc.Point = gpoints[i];
				pc.Parent = MyTransform;

				float distanceToCenter = (float)i / (float)pointCount;
				float val = Mathf.PerlinNoise (
					((pc.Point.Position.x / 100f) + 1f) / 2f * perlinScale,
					((pc.Point.Position.z / 100f) + 1f) / 2f * perlinScale
				);

				pc.SetFertility (distanceToCenter, val);
				if (Random.value < 0.08f) {
					pc.SetObject<DrillablePlot> ();
				} else {
					pc.SetObject<Plot> ();
				}

				pc.index = i;
				points.Add (pc);

				if (i % blockSize == 0)
					yield return null;
			}

			if (OnLoadPoints != null)
				OnLoadPoints ();
		}

		void ApplySeaLevel () {
			float seaLevel = sea.SeaLevel;
			List<PointContainer> newFlooded = Points.FindAll (x => x.Position.y < seaLevel);
			foreach (PointContainer p in flooded) {
				if (!newFlooded.Contains (p)) 
					p.SetFloodLevel (0);
			}
			foreach (PointContainer p in newFlooded) {
				p.SetFloodLevel (seaLevel - p.Position.y);
			}
			flooded = newFlooded;
		}
	}
}