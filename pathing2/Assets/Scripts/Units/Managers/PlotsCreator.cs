using UnityEngine;
using System.Collections;
using Pathing;

namespace Units {

	public class PlotsCreator : MonoBehaviour {

		Vector3 center = new Vector3 (0, 0.5f, 0);
		int sides = 5;
		int radius = 4;
		int rings = 5;

		void Awake () {
			CreatePlots ();
		}

		void CreatePlots () {
			for (int i = 0; i < rings; i ++) {
				CreateRing (i);
			}
		}

		void CreateRing (int index) {
			float myRadius = radius * index;
			int mySides = sides * (index);
			float deg = 360f / (float)mySides;
			for (int i = 0; i < mySides; i ++) {
				float radians = (float)i * deg * Mathf.Deg2Rad;
				Vector3 position = new Vector3 (
					center.x + myRadius * Mathf.Sin (radians),
					center.y,
					center.z + myRadius * Mathf.Cos (radians)
				); 
				if (index == 1 && i == 0) {
					CreateUnit<MilkPool> (position);
				} else {
					CreateUnit<Plot> (position);
				}
			}
		}

		void CreateUnit<T> (Vector3 position) where T : StaticUnit {
			PathPoint pathPoint = ObjectCreator.Instance.Create<PathPoint> (position).GetScript<PathPoint> ();
			T unit = ObjectCreator.Instance.Create<T> ().GetScript<T> ();
			unit.Position = position;
			unit.PathPoint = pathPoint;
			pathPoint.StaticUnit = unit as StaticUnit;
		}
	}
}