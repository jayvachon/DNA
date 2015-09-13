using UnityEngine;
using System.Collections;

namespace DNA.Units {

	public class UnitTransform : UnitComponent {
		protected override int ParentUnit { get { return 0; } }
		public virtual void OnSelect () {}
		public virtual void OnUnselect () {}
	}
}