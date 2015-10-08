using UnityEngine;
using System.Collections;

namespace DNA.EventSystem {

	public class MouseEnterPointEvent : ClickPointEvent {

		public MouseEnterPointEvent (PointContainer point) : base (point) {}	
	}
}