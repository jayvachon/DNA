using UnityEngine;
using System.Collections;
using Pathing;

namespace Units {

	public class PlotsCreator : MonoBehaviour {

		[Range (0.1f, 10f)] public float radius2 = 1.25f;
		[Range (0f, 1f)] public float altitude = 0.05f;
		public Vector3 origin = new Vector3 (0, 6.5f, 0);

		Fermat fermat = new Fermat ();
		int pointCount;
		Unit[] plots;

		void Awake () {
			pointCount = fermat.Points.Length;
			plots = new Unit[pointCount];
			SetPointPositions ();
		}

		void Update () {
			fermat.UpdateSettings (
				new Fermat.Settings (radius2, pointCount, altitude, origin));
			SetPointPositions ();
		}

		void SetPointPositions () {
			Vector3[] points = fermat.Points;
			for (int i = 0; i < points.Length; i ++) {
				if (plots[i] == null) {
					plots[i] = CreateUnitAtIndex (points[i], i);
				}
				plots[i].Position = points[i];
				((StaticUnit)plots[i]).PathPoint.Position = points[i];
				plots[i].transform.SetParent (transform);
			}
		}

		Unit CreateUnitAtIndex (Vector3 position, int index) {
			switch (index) {
				case 0: return CreateUnit<GivingTreeUnit> (position);
				case 1: return CreateUnit<CoffeePlant> (position);
				case 20: return CreateUnit<MilkshakePool> (position);
				case 40: return CreateUnit<MilkshakePool> (position);
				case 60: return CreateUnit<MilkshakePool> (position);
				default: return CreateUnit<Plot> (position);
			}
		}

		Unit CreateUnit<T> (Vector3 position) where T : StaticUnit {
			T unit = ObjectCreator.Instance.Create<T> ().GetScript<T> ();
			PathPoint pathPoint = Path.CreatePoint (position, unit as StaticUnit);
			unit.Position = position; 
			unit.PathPoint = pathPoint;
			return (Unit)unit;
		}
	}
}