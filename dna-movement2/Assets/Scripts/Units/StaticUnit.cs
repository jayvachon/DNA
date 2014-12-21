using UnityEngine;
using System.Collections;

public class StaticUnit : Unit, PathPoint {

	bool activated = true;
	public bool Activated {
		get { return activated; }
	}

	Vector3 position = ExtensionMethods.NullPosition;
	public Vector3 Position {
		get { 
			if (position == ExtensionMethods.NullPosition) {
				position = StartPosition;
			}
			return position; 
		}
	}
}
