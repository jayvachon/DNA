using UnityEngine;
using System.Collections;
using DNA.Paths.Dijkstra;

namespace DNA.Paths {

	public class Connection : PathElement {

		public GridPoint[] Points { get; set; }
		public Road Road { get; set; }

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

		Path<GridPoint>[] path;
		public Path<GridPoint>[] Path {
			get {
				if (path == null) {

					Path<GridPoint> p = new Path<GridPoint> () {
						Source = Points[0],
						Destination = Points[1],
						Cost = (int)Vector3.Distance (Positions[0], Positions[1])
					};

					path = new [] {
						p,
						new Path<GridPoint> () {
							Source = p.Destination,
							Destination = p.Source,
							Cost = p.Cost
						}
					};
				}
				return path;
			}
		}

		float length = -1;
		public float Length {
			get {
				if (Mathf.Approximately (-1, length)) {
					length = Vector3.Distance (Positions[0], Positions[1]);
				}
				return length;
			}
		}

		public bool ContainsPoints (GridPoint a, GridPoint b) {
			return Points[0] == a && Points[1] == b || Points[0] == b && Points[1] == a;
		}

		public int Cost {
			get { return Path[0].Cost; }
			set {
				Path[0].Cost = value;
				Path[1].Cost = value;
				UpdateVersion ();
				if (onUpdateCost != null)
					onUpdateCost (value);
			}
		}

		public void SetFree () {
			Cost = 0;
		}

		public void DisablePath () {
			Cost = int.MaxValue;
		}

		public delegate void OnUpdateCost (int cost);

		public OnUpdateCost onUpdateCost;
	}
}