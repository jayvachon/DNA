using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pathing;
using Units;

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
		
		List<StaticUnit> matches = StaticUnits.FindAll (x => x.GetType () == unitType);
		if (matches.Count == 0) 
			return null;

		float nearestDistance = Mathf.Infinity;
		StaticUnit nearestUnit = matches[0];
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
