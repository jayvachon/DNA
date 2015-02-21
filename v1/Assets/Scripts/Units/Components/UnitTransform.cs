using UnityEngine;
using System.Collections;
using Pathing;

public class UnitTransform : MBRefs, IPathMover {

	public Vector3 Position {
		get { return MyTransform.position; }
		set { MyTransform.position = value; }
	}
}
