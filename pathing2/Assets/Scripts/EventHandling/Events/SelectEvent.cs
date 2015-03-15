using UnityEngine;
using System.Collections;
using GameInput;
using Units;

namespace GameEvents {

	public class SelectEvent : GameEvent {

		public readonly ISelectable selected = null;
		public readonly Unit unit = null;

		public SelectEvent (ISelectable selected) {
			
			this.selected = selected;

			UnitClickable clickable = selected as UnitClickable;
			if (clickable != null) {
				unit = clickable.Unit;
			}
		}
	}
}