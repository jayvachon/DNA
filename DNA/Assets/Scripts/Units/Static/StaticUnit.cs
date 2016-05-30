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

				UpdateAccessbility ();

				foreach (Connection connection in Point.Connections)
					connection.onUpdateCost += OnUpdateConnectionCost;
			}
		}

		GridPoint Point {
			get { 
				// try {
					return (GridPoint)Element; 
				/*} catch (System.InvalidCastException e) {
					throw new System.Exception ("Could not find the GridPoint for " + this + "\n" + e);
				}*/
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

		protected override void OnEnable () {
			base.OnEnable ();
		}

		protected override void OnDisable () {

			base.OnDisable ();

			if (Element != null) {
				Element.OnSetState -= OnSetState;
				foreach (Connection connection in Point.Connections)
					connection.onUpdateCost -= OnUpdateConnectionCost;
			}
		}

		protected virtual void OnSetState (DevelopmentState state) {
			if (state == DevelopmentState.Abandoned)
				unitRenderer.SetAbandoned ();
		}

		void UpdateAccessbility () {

			ILaborDependent ld = this as ILaborDependent;

			if (ld != null) {
				Debug.Log (this is ConstructionSite);
				ld.Accessible = Point.HasRoad;
			}
		}

		void OnUpdateConnectionCost (int cost) {
			UpdateAccessbility ();
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

				List<Laborer> laborers = UnitManager.GetUnitsOfType<Laborer> ();
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