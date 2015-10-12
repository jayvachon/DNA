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

			for (int i = 0; i < gpoints.Count; i ++) {
				PointContainer pc = ObjectPool.Instantiate<PointContainer> ();
				pc.Point = gpoints[i];
				pc.Parent = MyTransform;
				pc.SetObject<DrillablePlot> ();
				points.Add (pc);
			}
		}
	}
}