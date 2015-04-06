using UnityEngine;
using System.Collections;

namespace GameActions {
	
	public abstract class Action : INameable {

		public virtual string Name {
			get { return ""; }
		}

		bool enabled = true;
		public virtual bool Enabled {
			get { return enabled; }
			set { enabled = value; }
		}
	}
}