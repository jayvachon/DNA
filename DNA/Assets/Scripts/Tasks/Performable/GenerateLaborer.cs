#undef IGNORE_HOUSES
using UnityEngine;
using System.Collections;
using DNA.Units;

namespace DNA.Tasks {

	public class GenerateLaborer : GenerateUnit<Laborer> {

		public override bool Enabled {
			get { return CanAfford 
				#if IGNORE_HOUSES
				;
				#else
				&& !Player.Instance.Inventory["Laborer"].Full; 
				#endif
			}
		}
	}
}