using UnityEngine;
using System.Collections;

namespace DNA.Paths {

	public interface IPathElementVisitor {
		int VisitorIndex { get; set; }
	}
}