using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DNA.Models;
using DNA.Paths;
using DNA.Units;
using InventorySystem;

namespace DNA.Tasks {

	public class ConstructRoad : ConstructUnit {

		public override bool CanConstruct (PathElement element) {
			Connection c = element as Connection;
			return c != null
				&& CanAfford 
				&& c.State == DevelopmentState.Undeveloped 
				&& c.Cost > 0 
				&& System.Array.Find (c.Points, x => x.HasRoad || x.HasRoadConstruction) != null;
		}

		protected override void OnEnd () {
			Purchase ();
			ConnectionContainer c = (ConnectionContainer)ElementContainer;
			c.BeginConstruction<Road> (TotalCost);
			base.OnEnd ();
		}
	}
}