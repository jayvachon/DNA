using UnityEngine;
using System.Collections;

namespace Pathing {

	public interface IPathable {
		Path Path { get; set; }
		float Progress { get; set; }
		bool StartMovingOnPath ();
		void StopMovingOnPath ();
		void ArriveAtPoint (PathPoint pathPoint);
	}
}