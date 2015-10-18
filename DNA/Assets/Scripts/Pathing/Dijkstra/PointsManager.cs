using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DNA.Units;

namespace DNA.Paths {

	public class PointsManager : MBRefs {

		readonly List<PointContainer> points = new List<PointContainer> ();

		public List<PointContainer> Points {
			get { return points; }
		}

		public void Init () {
			CreatePoints ();
		}

		public void SetUnitAtIndex<T> (int index) where T : StaticUnit {
			points[index].BeginConstruction<T> ();
			points[index].EndConstruction ();
		}

		public List<Connection> GetConnectionsAtIndex (int index) {
			return points[index].Point.Connections;
		}

		void CreatePoints () {
			
			List<GridPoint> gpoints = TreeGrid.Points;
			int pointCount = gpoints.Count;

			for (int i = 0; i < pointCount; i ++) {
				PointContainer pc = ObjectPool.Instantiate<PointContainer> ();
				pc.Point = gpoints[i];
				pc.Parent = MyTransform;
				DrillablePlot p = pc.SetObject<DrillablePlot> ();

				float progress = (float)i / (float)pointCount;
				p.DistanceToCenter = progress;
				float x = ((pc.Point.Position.x / 100f) + 1f) / 2f;
				float y = ((pc.Point.Position.z / 100f) + 1f) / 2f;
				Debug.Log (x + ", " + y);
				p.Fertility = Mathf.PerlinNoise (x, y);
				//p.Fertility = Mathf.PerlinNoise (progress, progress);
				//p.Fertility = Random.value;
				
				points.Add (pc);
			}
		}
	}
}