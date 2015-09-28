using UnityEngine;
using System.Collections;
using DNA.Paths.Dijkstra;

namespace DNA.Paths {

	public class Connection : PathElement {

		public GridPoint[] Points { get; set; }

		Vector3[] positions;
		public Vector3[] Positions {
			get {
				if (positions == null) {
					positions = new Vector3[2];
					positions[0] = Points[0].Position;
					positions[1] = Points[1].Position;
				}
				return positions;
			}
		}
		
		// Might be better just to use the 2way property (this one might be extraneous - not being used by anything atm)
		Path<GridPoint> path;
		public Path<GridPoint> Path {
			get {
				if (path == null) {
					path = new Path<GridPoint> () {
						Source = Points[0],
						Destination = Points[1],
						Cost = (int)Vector3.Distance (Positions[0], Positions[1])
					};
				}
				return path;
			}
		}

		Path<GridPoint>[] path2Way;
		public Path<GridPoint>[] Path2Way {
			get {
				if (path2Way == null) {
					path2Way = new [] {
						Path,
						new Path<GridPoint> () {
							Source = Path.Destination,
							Destination = Path.Source,
							Cost = Path.Cost
						}
					};
				}
				return path2Way;
			}
		}

		public int Cost {
			get { return Path.Cost; }
			set {
				Path.Cost = value;
				Path2Way[0].Cost = value;
				Path2Way[1].Cost = value;
				UpdateVersion ();
			}
		}

		public void SetFree () {
			Cost = 0;
		}

		public void DisablePath () {
			Cost = int.MaxValue;
		}
	}
}