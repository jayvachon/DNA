using UnityEngine;
using System.Collections;
using DNA.Paths;

namespace DNA.EventSystem {

	public class MouseEnterPointEvent : ClickPointEvent {

		public MouseEnterPointEvent (PointContainer point) : base (point) {}	
	}
}