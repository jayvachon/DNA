using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DNA.Paths;
using DNA.Units;

namespace DNA {

	public class PointsManager : MBRefs {

		new void Awake () {
			CreatePoints ();
		}

		void CreatePoints () {
			
			List<GridPoint> gpoints = TreeGrid.Points;

			for (int i = 0; i < gpoints.Count; i ++) {
				PointContainer pc = ObjectCreator.Instance.Create<PointContainer> ().GetScript<PointContainer> ();
				pc.Point = gpoints[i];
				pc.Parent = MyTransform;
				pc.SetStaticUnit<DrillablePlot> ();
			}
		}
	}
}