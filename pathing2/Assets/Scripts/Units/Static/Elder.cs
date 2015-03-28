using UnityEngine;
using System.Collections;
using GameActions;

namespace Units {

	public class Elder : StaticUnit, IActionAcceptor, IActionPerformer {
		
		public override string Name {
			get { return "Elder"; }
		}		

		public AcceptableActions AcceptableActions { get; private set; }
		public PerformableActions PerformableActions { get; private set; }

		void Awake () {

			// not needed?
			AcceptableActions = new AcceptableActions (this);
			PerformableActions = new PerformableActions (this);
		}
	}
}