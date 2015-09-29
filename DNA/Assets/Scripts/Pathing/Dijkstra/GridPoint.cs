using UnityEngine;
using System.Collections;
using DNA.Units;

namespace DNA.Paths {

	public class GridPoint : PathElement {

		public Vector3 Position { get; set; }
		public StaticUnit Unit { get; set; }

	}
}