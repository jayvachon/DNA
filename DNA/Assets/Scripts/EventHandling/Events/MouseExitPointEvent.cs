using UnityEngine;
using System.Collections;
using DNA.Paths;

namespace DNA.EventSystem {

	public class MouseExitPointEvent : ClickPointEvent {

		public MouseExitPointEvent (PointContainer point) : base (point) {}	
	}
}