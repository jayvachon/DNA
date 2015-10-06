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

		void OnEnable () { PlayerActionState.onChange += OnChangeState; }
		void OnDisable () { PlayerActionState.onChange -= OnChangeState; }

		public void Init () {
			CreatePoints ();
		}

		public void SetUnitAtIndex<T> (int index) where T : StaticUnit {
			points[index].SetStaticUnit<T> ();
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
				pc.SetStaticUnit<DrillablePlot> ();
				points.Add (pc);
			}
		}

		void EnableHighlighting () {
			foreach (PointContainer point in points)
				point.EnableHighlighting ();
		}

		void DisableHighlighting () {
			foreach (PointContainer point in points)
				point.DisableHighlighting ();
		}

		void OnChangeState (ActionState state) {
			if (state == ActionState.RoadConstruction || state == ActionState.BuildingConstruction)
				EnableHighlighting ();
			else
				DisableHighlighting ();
		}
	}
}