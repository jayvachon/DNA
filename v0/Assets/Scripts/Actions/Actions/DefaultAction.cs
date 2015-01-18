using UnityEngine;
using System.Collections;

// don't think this gets used, kill it?
namespace GameActions {

	public class DefaultAction : Action {

		public override string Name {
			get { return "Default"; }
		}
		
		public DefaultAction () : base (0) {}
	}
}