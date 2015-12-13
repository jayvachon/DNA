using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DNA.Paths;
using DNA.Tasks;
using DNA.InputSystem;
using InventorySystem;

namespace DNA.Units {

	public class ConstructionSite : StaticUnit, IPathElementObject {

		public int LaborCost {
			get { return Inventory["Labor"].Count; }
			set { Inventory["Labor"].Set (value); }
		}

		public override SelectSettings SelectSettings {
			get { 
				if (selectSettings == null) {
					selectSettings = new SelectSettings (
						new List<System.Type> () {
							typeof (Ground),
							typeof (DNA.Paths.ConnectionContainer),
							typeof (FogOfWar)
						}
					);
				}
				return selectSettings;
			}
		}

		public RoadPlan RoadPlan { get; set; }

		void Awake () {
			unitRenderer.SetColors (new Color (1f, 1f, 1f));
		}

		protected override void OnInitInventory (Inventory i) {
			i.Add (new LaborGroup ());
		}

		protected override void OnInitAcceptableTasks (AcceptableTasks a) {
			a.Add (new AcceptCollectItem<LaborGroup> ());
		}
	}
}