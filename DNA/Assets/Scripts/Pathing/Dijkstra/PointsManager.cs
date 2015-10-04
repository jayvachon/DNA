using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DNA.Units;

namespace DNA.Paths {

	public class PointsManager : MBRefs {

		new void Awake () {
			CreatePoints ();
		}

		void CreatePoints () {
			
			List<GridPoint> gpoints = TreeGrid.Points;

			for (int i = 0; i < gpoints.Count; i ++) {
				PointContainer pc = ObjectPool.Instantiate<PointContainer> ();
				pc.Point = gpoints[i];
				pc.Parent = MyTransform;
				CreateStaticUnit (pc, i);
			}
		}

		void CreateStaticUnit (PointContainer pc, int index) {
			switch (index) {
				case 0:		pc.SetStaticUnit<GivingTreeUnit> (); break;
				default: 	pc.SetStaticUnit<DrillablePlot> (); break;
			}
		}
	}
}