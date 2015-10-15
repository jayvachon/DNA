using UnityEngine;
using System.Collections;
using DNA.Paths;

namespace DNA {

	public class ConstructionSite : MBRefs, IPathElementObject {

		public PathElement Element { get; set; }
	}
}