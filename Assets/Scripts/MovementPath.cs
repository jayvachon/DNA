using UnityEngine;
using System.Collections;

public class MovementPath {

	// Settings
	protected int stepSize = 5;
	protected bool ignoreY = true;

	// The generated path
	protected Vector3[] path;
	public Vector3[] Path {
		get { return path; }
	}

	public virtual void CreatePath () {}
}
