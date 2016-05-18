using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using DNA.InputSystem;
using DNA.EventSystem;
using DNA.Tasks;
using DNA.Paths;

namespace DNA.Units {

	public class StaticUnit : Unit, IPathElementObject {

		PathElement element;
		public PathElement Element { 
			get { return element; }
			set {
				element = value;
				element.OnSetState += OnSetState;
			}
		}

		public PathElementContainer Container { get; set; }

		int fertilityTier;
		public int FertilityTier {
			get { return fertilityTier; }
			set { 
				fertilityTier = value;
				OnSetFertility (value);
			}
		}

		protected override void OnDisable () {
			base.OnDisable ();
			if (Element != null)
				Element.OnSetState -= OnSetState;
		}

		protected virtual void OnSetState (DevelopmentState state) {
			if (state == DevelopmentState.Abandoned)
				unitRenderer.SetAbandoned ();
		}

		// TODO: update this to work with new points
		// this happens e.g. when coffee runs out of resources
		protected void Destroy<T> (bool enablePathPoint=true) where T : StaticUnit {
			if (enablePathPoint) {
				StaticUnit plot = UnitManager.Instantiate<Plot> () as StaticUnit;
				plot.Position = Position;
				if (Selected) SelectionManager.Select (plot.UnitClickable);
			}
			DestroyThis<T> ();
		}

		public override void OnPointerDown (PointerEventData e) {
			Events.instance.Raise (new PointerDownEvent (this, e));
			if (!e.LeftClicked () && SelectionHandler.Selected.Count == 0) {

				// TODO: use UnitManager for lookup
				List<Laborer> laborers = ObjectPool.GetActiveInstances<Laborer> ();
				Laborer available = laborers.Find (x => x.Idle);
				if (available != null) {
					available.OnOverrideSelect (this);
				}
			}
		}

		protected virtual void OnSetFertility (int fertility) {}

		#region ISelectable implementation
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
		#endregion
	}
}