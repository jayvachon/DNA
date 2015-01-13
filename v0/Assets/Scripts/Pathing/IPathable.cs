using UnityEngine;
using System.Collections;

namespace Pathing {
	
	public interface IPathable {

		Path MyPath { get; }
		bool ActivePath { set; }

		void OnUpdatePath ();
		void StartMoveOnPath ();
		void OnArriveAtPoint ();
	}
}