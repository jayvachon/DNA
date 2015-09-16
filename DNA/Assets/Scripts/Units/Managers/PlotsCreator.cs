#undef DYNAMIC_SETTINGS
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pathing;

namespace DNA.Units {

	// TODO: remove fog of war from here and use TreeGrid to create plots

	public class PlotsCreator : MonoBehaviour {

		#if DYNAMIC_SETTINGS
		[Range (0.1f, 10f)] public float radius = 1.25f;
		[Range (0f, 1f)] public float altitude = 0.05f;
		public Vector3 origin = new Vector3 (0, 6.5f, 0);
		#else
		float radius = 1.25f;
		float altitude = 0.05f;
		Vector3 origin = new Vector3 (0, 6.5f, 0);
		#endif

		Fermat fermat = new Fermat ();
		Fermat fowFermat = new Fermat ();
		int pointCount;
		Unit[] plots;

		void Awake () {
			pointCount = fermat.Points.Length;
			plots = new Unit[pointCount];
			fermat.UpdateSettings (
				new Fermat.Settings (radius, pointCount, altitude, origin));
			fowFermat.UpdateSettings (
				new Fermat.Settings (radius + 0.25f, pointCount, altitude, origin));
			SetPointPositions ();
		}

		#if DYNAMIC_SETTINGS
		void Update () {
			fermat.UpdateSettings (
				new Fermat.Settings (radius, pointCount, altitude, origin));
			SetPointPositions ();
		}
		#endif

		void SetPointPositions () {
			Vector3[] points = fermat.Points;
			Vector3[] fowPoints = fowFermat.Points;
			for (int i = 0; i < points.Length; i ++) {
				if (plots[i] == null) {
					plots[i] = CreateUnitAtIndex (points[i], i);
				}
				if (i > 20)
					CreateFogOfWar (fowPoints[i]);
				plots[i].Position = points[i];
				((StaticUnit)plots[i]).PathPoint.Position = points[i];
				plots[i].transform.SetParent (transform);
			}
			GeneratePaths ();
		}

		void CreateFogOfWar (Vector3 position) {
			//ObjectCreator.Instance.Create<FogOfWarParticles> (position);
		}

		Unit CreateUnitAtIndex (Vector3 position, int index) {
			switch (index) {
				case 0: return (Unit)CreateUnit<GivingTreeUnit> (position);
				case 4: return (Unit)CreateUnit<MilkshakePool> (position);
				/*case 20: return (Unit)CreateUnit<MilkshakePool> (position);
				case 40: return (Unit)CreateUnit<MilkshakePool> (position);
				case 60: return (Unit)CreateUnit<MilkshakePool> (position);*/
				default: 
					DrillablePlot plot = CreateUnit<DrillablePlot> (position);
					plot.PositionInSpiral = (float)index / (float)pointCount;
					plot.Index = index;
					return (Unit)plot;
				/*default: return (Random.Range (0, (int)(pointCount/2)) < index) 
					? CreateUnit<FertilePlot> (position)
					: CreateUnit<Plot> (position);*/
			}
		}

		T CreateUnit<T> (Vector3 position) where T : StaticUnit {
			T unit = ObjectCreator.Instance.Create<T> ().GetScript<T> ();
			PathPoint pathPoint = Path.CreatePoint (position, unit as StaticUnit);
			unit.Position = position; 
			unit.PathPoint = pathPoint;
			return unit;
		}

		void GeneratePaths () {
			for (int i = 0; i < plots.Length; i ++) {
				DrillablePlot p = plots[i] as DrillablePlot;
				if (p != null) {
					Unit a = (i + 21 < plots.Length) ? plots[i+21] : null;
					Unit b = (i + 13 < plots.Length) ? plots[i+13] : null;
					Unit c = (i < 14) ? plots[0] : null;
				}
			}
		}
	}
}