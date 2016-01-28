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
			set { 
				Inventory["Labor"].Capacity = value;
				Inventory["Labor"].Fill ();
			}
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

		ProgressBar pbar;

		protected override void OnEnable () {
			base.OnEnable ();
			Coroutine.WaitForFixedUpdate (() => {
				if (gameObject.activeSelf) {
					GridPoint gp = Element as GridPoint;
					if (gp != null) {
						pbar = UI.Instance.CreateProgressBar (gp.Position);
					} else {
						Connection c = Element as Connection;
						if (c != null)
							pbar = UI.Instance.CreateProgressBar (c.Center);
					}
				}
			});
		}
		
		protected override void OnDisable () { 
			base.OnDisable ();
			if (UI.Instance != null) {
				UI.Instance.DestroyProgressBar (pbar); 
				pbar = null;
			}
		}

		protected override void OnInitInventory (Inventory i) {
			i.Add (new LaborGroup ()).onUpdate += OnUpdateLabor;
		}

		protected override void OnInitAcceptableTasks (AcceptableTasks a) {
			a.Add (new AcceptCollectItem<LaborGroup> ());
		}

		void OnUpdateLabor () {
			if (pbar != null)
				pbar.SetProgress (Inventory["Labor"].PercentFilled);
		}
	}
}