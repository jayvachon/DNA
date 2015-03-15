using UnityEngine;
using System.Collections;

namespace Pathing {

	public interface IPathable {
		Path Path { get; set; }
		Vector3 Position { get; set; }
		void StartMoveOnPath ();
		void ArriveAtPoint (PathPoint pathPoint);
	}
}