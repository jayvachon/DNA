using UnityEngine;
using System.Collections;

public interface PathPoint {

	bool Activated { get; }
	Vector3 Position { get; }
}
