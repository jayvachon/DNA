using UnityEngine;
using System.Collections;

namespace Pathing {

	public interface IPathable {
		Path Path { get; set; }
		Vector3 PathPosition { get; set; }
		void StartMovingOnPath ();
		void StopMovingOnPath ();
		void ArriveAtPoint (PathPoint pathPoint);
	}
}