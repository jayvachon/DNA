﻿using UnityEngine;
using System.Collections;
using GameInput;

namespace Units {

	public class StaticUnitClickable : UnitClickable {

		public override void OnClick (ClickSettings clickSettings) {
			if (!CanSelect) return;
			if (clickSettings.left) {
				SelectionManager.Select (this);
			} else {
				if (SelectionManager.IsSelected (this)) {
					SelectionManager.Unselect ();
				} else {
					// assign action to selected mobile unit
				}
			}
		}		
	}
}