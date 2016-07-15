using UnityEngine;
using System.Collections;

namespace DNA.Units {

	public interface IWorkplace {
		bool Accessible { get; set; }	// Can workers reach this building? (is it connected by roads)
		float Efficiency { get; set; }	// How efficiently does this building work? (what is the ratio of workers:buildings)
		void OnUpdateEfficiency ();
	}
}