using UnityEngine;
using System.Collections;

namespace GameActions {

	public class CancelGenerateUnit : PerformerAction {

		public override string Name {
			get { return "CancelGenerateUnit"; }
		}

		public CancelGenerateUnit () : base (0f) {}
	}
}