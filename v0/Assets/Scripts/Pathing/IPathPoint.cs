using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Pathing {
	
	public interface IPathPoint {

		Vector3 Position { get; }
		List<Path> Paths { get; set; }
	}
}