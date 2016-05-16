using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using DNA.InputSystem;
using DNA.EventSystem;
using DNA.Models;
using DNA.Tasks;
using InventorySystem;

namespace DNA.Units {

	public class Unit : MBRefs, INameable, IInventoryHolder, ISelectable, IPointerDownHandler, ITaskPerformer, ITaskAcceptor {

		public virtual string Name {
			get { return Settings.Title; }
		}

		public virtual string Description {
			get { return Settings.Description; }
		}

		public UnitSettings settings;
		public UnitSettings Settings {
			get {
				if (settings == null) {
					settings = DataManager.GetUnitSettings (this.GetType ());
				}
				return settings;
			}
		}

		public UnitTransform unitTransform; // deprecate
		public UnitRenderer unitRenderer;
		public UnitClickable unitClickable; // deprecate

		Inventory inventory;
		public Inventory Inventory {
			get {
				if (inventory == null) {
					inventory = new Inventory (this);
					OnInitInventory (inventory);
				}
				return inventory;
			}
			protected set {
				inventory = value;
				OnInitInventory (inventory);
			}
		}

		PerformableTasks performableTasks;
		public PerformableTasks PerformableTasks {
			get {
				if (performableTasks == null) {
					performableTasks = new PerformableTasks (this);
					OnInitPerformableTasks (performableTasks);
				}
				return performableTasks;
			}
		}

		AcceptableTasks acceptableTasks;
		public AcceptableTasks AcceptableTasks {
			get {
				if (acceptableTasks == null) {
					acceptableTasks = new AcceptableTasks (this);
					OnInitAcceptableTasks (acceptableTasks);
				}
				return acceptableTasks;
			}
		}
		
		protected bool Selected { get; private set;}

		protected UnitInfoContent unitInfoContent = null;
		public UnitInfoContent UnitInfoContent { 
			get {
				if (unitInfoContent == null) {
					unitInfoContent = new UnitInfoContent (this);
				}
				return unitInfoContent;
			}
		}

		// deprecate
		public UnitClickable UnitClickable {
			get { return unitClickable; }
		}

		public UnitTransform UnitTransform {
			get { return unitTransform; }
		}

		public Transform Transform {
			get { return unitTransform.MyTransform; }
		}

		// Changes this unit T to unit U
		protected void ChangeUnit<T, U>  () where T : Unit where U : Unit {
			U to = ObjectPool.Instantiate<U> ();
			to.Position = Position;
			if (Selected) {
				SelectionManager.Select (to.UnitClickable);
			}
			OnChangeUnit (to);
			DestroyThis<T> ();
		}

		protected virtual void OnChangeUnit<U> (U u) where U : Unit {}

		protected virtual void OnEnable () {
			EmissionsManager.AddEmissions (Settings.Emissions);
			string renderer = UnitRenderer.GetRenderer (Settings.Symbol);
			if (renderer != "") {
				unitRenderer = ObjectPool.Instantiate (renderer) as UnitRenderer;
				unitRenderer.transform.SetParent (MyTransform);
				unitRenderer.transform.localPosition = unitRenderer.Offset;
				unitRenderer.transform.localEulerAngles = Vector3.zero;
			}
		}

		protected virtual void OnDisable () {
			EmissionsManager.RemoveEmissions (Settings.Emissions);
			if (Selected) {
				SelectionManager.Unselect ();
			}
			string renderer = UnitRenderer.GetRenderer (Settings.Symbol);
			if (renderer != "") {
				if (unitRenderer != null) {
					ObjectPool.Destroy (unitRenderer);
					unitRenderer = null;
				}
			}
		}

		protected void RefreshInfoContent () {
			UnitInfoContent.Refresh ();
		}

		protected void DestroyThis<T> () where T : Unit {
			ObjectPool.Destroy<T> (transform);
		}

		protected virtual void OnInitInventory (Inventory i) {}
		protected virtual void OnInitPerformableTasks (PerformableTasks p) {}
		protected virtual void OnInitAcceptableTasks (AcceptableTasks a) {}

		#region ISelectable implementation
		protected SelectSettings selectSettings;
		public virtual SelectSettings SelectSettings {
			get { 
				if (selectSettings == null) {
					selectSettings = new SelectSettings (
						new List<System.Type> () {
							typeof (Ground),
							// typeof (DNA.Paths.ConnectionContainer),
							typeof (FogOfWar)
						}
					);
				}
				return selectSettings;
			}
		}

		public virtual void OnSelect () {
			Selected = true;
			// TODO: entirely remove renderer and transform			
			if (unitRenderer != null)
				unitRenderer.OnSelect ();
			if (unitTransform != null)
				unitTransform.OnSelect ();
		}

		public virtual void OnUnselect () {
			Selected = false;
			// TODO: entirely remove renderer and transform			
			if (unitRenderer != null)
				unitRenderer.OnUnselect ();
			if (unitTransform != null)
				unitTransform.OnUnselect ();
		}
		#endregion

		#region IPointerDownHandler implementation
		public virtual void OnPointerDown (PointerEventData e) {
			Events.instance.Raise (new PointerDownEvent (this, e));
			SelectionHandler.ClickSelectable (this, e);
		}
		#endregion
	}
}