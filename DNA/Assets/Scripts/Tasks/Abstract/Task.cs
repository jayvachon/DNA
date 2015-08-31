using UnityEngine;
using System.Collections;

namespace DNA.Tasks {

	public class Task {

		bool enabled = true;
		public virtual bool Enabled { 
			get { return enabled; }
			set { enabled = value; }
		}
	}
}