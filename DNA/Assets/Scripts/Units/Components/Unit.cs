using UnityEngine;
using System.Collections;
using DNA.InputSystem;
using InventorySystem;
using DNA.Models;

namespace DNA.Units {

	public class Unit : MBRefs, INameable, IInventoryHolder, ISelectable {

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

		public UnitTransform unitTransform;
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

		/*public Vector3 Position {
			get { return Transform.position; }
			set { unitTransform.Position = value; }
		}*/

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
			if (UnitRenderer.Renderers.ContainsKey(Settings.Symbol)) {
				unitRenderer = ObjectPool.Instantiate (UnitRenderer.Renderers[Settings.Symbol]) as UnitRenderer;
				unitRenderer.transform.SetParent (MyTransform);
				unitRenderer.transform.localPosition = unitRenderer.Offset;
			}
		}

		protected virtual void OnDisable () {
			EmissionsManager.RemoveEmissions (Settings.Emissions);
			if (Selected) {
				SelectionManager.Unselect ();
			}
			if (UnitRenderer.Renderers.ContainsKey(Settings.Symbol)) {
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

		#region ISelectable implementation
		SelectSettings selectSettings;
		public SelectSettings SelectSettings {
			get { 
				if (selectSettings == null) {
					selectSettings = new SelectSettings ();
				}
				return selectSettings;
			}
		}

		public virtual void OnSelect () {
			Selected = true;
			unitRenderer.OnSelect ();
			unitTransform.OnSelect ();
		}

		public virtual void OnUnselect () {
			Selected = false;
			unitRenderer.OnUnselect ();
			unitTransform.OnUnselect ();
		}
		#endregion
	}
}