using UnityEngine;
using System.Collections;
//using DNA.InventorySystem;
using DNA.InputSystem;
using InventorySystem;

namespace DNA.Units {

	public class Unit : MonoBehaviour, INameable, IInventoryHolder, ISelectable {

		public virtual string Name {
			get { return ""; }
		}

		public virtual string Description {
			get { return ""; }
		}

		public UnitTransform unitTransform;
		public UnitRenderer unitRenderer;
		public UnitClickable unitClickable;

		#if UNITY_EDITOR
			// Ostensibly so that the inventory can be viewed in the inspector
			public Inventory inventory;
			public Inventory Inventory {
				get {
					return inventory;
				}
				protected set {
					inventory = value;
				}
			}
		#else
			public Inventory Inventory { get; protected set; }
		#endif
		
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

		public UnitClickable UnitClickable {
			get { return unitClickable; }
		}

		public UnitTransform UnitTransform {
			get { return unitTransform; }
		}

		public Transform Transform {
			get { return unitTransform.MyTransform; }
		}

		public Vector3 Position {
			get { return Transform.position; }
			set { unitTransform.Position = value; }
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
			EmissionsManager.AddUnit (this);
		}

		protected virtual void OnDisable () {
			EmissionsManager.RemoveUnit (this);
			if (Selected) {
				SelectionManager.Unselect ();
			}
		}

		protected void RefreshInfoContent () {
			UnitInfoContent.Refresh ();
		}

		protected void DestroyThis<T> () where T : Unit {
			ObjectPool.Destroy<T> (transform);
		}

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
			UnitInfoBoxOverlay.Instance.Open (UnitInfoContent);
		}

		public virtual void OnUnselect () {
			Selected = false;
			unitRenderer.OnUnselect ();
			unitTransform.OnUnselect ();
			UnitInfoBoxOverlay.Instance.Close ();
		}
		#endregion
	}
}