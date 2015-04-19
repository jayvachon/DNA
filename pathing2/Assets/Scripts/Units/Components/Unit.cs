using UnityEngine;
using System.Collections;
using GameInventory;
using GameActions;
using GameInput;

namespace Units {

	public class Unit : MonoBehaviour, INameable, IPoolable, IInventoryHolder {
		
		public virtual string Name {
			get { return ""; }
		}

		public UnitTransform unitTransform;
		public UnitRenderer unitRenderer;
		public UnitClickable unitClickable;

		#if UNITY_EDITOR
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

		public virtual void OnSelect () {
			Selected = true;
			unitRenderer.OnSelect ();
			unitTransform.OnSelect ();
			UnitInfoBox.Instance.Open (UnitInfoContent, Transform);
		}

		public virtual void OnUnselect () {
			Selected = false;
			unitRenderer.OnUnselect ();
			unitTransform.OnUnselect ();
			UnitInfoBox.Instance.Close ();
		}

		public virtual void OnPoolDestroy () {
			if (Selected) {
				SelectionManager.Unselect ();
			}
		}

		public virtual void OnPoolCreate () {}
	}
}