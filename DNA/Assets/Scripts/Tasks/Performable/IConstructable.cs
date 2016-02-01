using UnityEngine;
using System.Collections;
using DNA.Paths;

namespace DNA.Tasks {

	public interface IConstructable {
		PathElementContainer ElementContainer { get; set; }
		bool CanConstruct (PathElement element);
		bool Start ();
	}
}