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

			List<GridPoint> gpoints = TreeGrid.Points;

			for (int i = 0; i < gpoints.Count; i ++) {

				PointContainer pc = ObjectPool.Instantiate<PointContainer> ();
				pc.Point = gpoints[i];
				pc.Parent = MyTransform;

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