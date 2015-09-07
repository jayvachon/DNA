﻿using UnityEngine;
using System.Collections;
using Units;
using GameInventory;

namespace DNA.Tasks {

	public class GenerateUnit<T> : CostTask where T : Unit {

		protected override void OnEnd () {
			Unit unit = ObjectCreator.Instance.Create<T> ().GetScript<Unit> ();
			base.OnEnd ();
		}
	}
}