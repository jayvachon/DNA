using UnityEngine;
using System.Collections;

namespace GameActions {

	public class CancelGenerateUnit : PerformerAction {

		public override string Name {
			get { return "CencelGenerateUnit"; }
		}

		public CancelGenerateUnit () : base (0f) {}
	}
}