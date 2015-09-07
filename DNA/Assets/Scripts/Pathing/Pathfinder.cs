using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pathing;
using Units;
using DNA.Tasks;

public class Pathfinder : MBRefs {

	static Pathfinder instance = null;
	static public Pathfinder Instance {
		get {
			if (instance == null) {
				instance = Object.FindObjectOfType (typeof (Pathfinder)) as Pathfinder;
				if (instance == null) {
					GameObject go = new GameObject ("Pathfinder");
					DontDestroyOnLoad (go);
					instance = go.AddComponent<Pathfinder>();
				}
			}
			return instance;
		}
	}

	List<StaticUnit> StaticUnits {
		get { return pathPoints.ConvertAll (x => x.StaticUnit); }
	}

	List<PathPoint> pathPoints = new List<PathPoint> ();

	public void AddPathPoint (PathPoint pathPoint) {
		pathPoints.Add (pathPoint);
	}

	public StaticUnit FindNearestStaticUnit (Vector3 position, System.Type unitType) {
		return FindNearest (position, StaticUnits.FindAll (x => x.GetType () == unitType));
	}

	public PathPoint FindNearestWithAction (Vector3 position, string action) {
		StaticUnit su = FindNearest (position, StaticUnits.FindAll (x => x.AcceptableActions.Has (action)));
		return (su == null) ? null : su.PathPoint;
	}

	StaticUnit FindNearest (Vector3 position, List<StaticUnit> matches) {
		float nearestDistance = Mathf.Infinity;
		StaticUnit nearestUnit = null;
		foreach (StaticUnit unit in matches) {
			float distance = Vector3.Distance (position, unit.Position);
			if (distance < nearestDistance) {
				nearestUnit = unit;
				nearestDistance = distance;
			}
		}
		return nearestUnit;
	}
}
