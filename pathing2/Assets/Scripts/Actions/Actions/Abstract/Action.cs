﻿using UnityEngine;
using System.Collections;

namespace GameActions {
	
	public abstract class Action : INameable {

		// TODO: Rename to ID
		public virtual string Name {
			get { return ""; }
		}

		public virtual bool Enabled {
			get { return EnabledState.Enabled; }
		}

		public virtual EnabledState EnabledState { 
			get { return new DefaultEnabledState (); }
		}

		bool active = true;
		public virtual bool Active {
			get { return active; }
			set { active = value; }
		}
	}
}