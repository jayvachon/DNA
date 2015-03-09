using UnityEngine;
using System.Collections;
using GameInventory;

namespace Units {

	public class Unit : MonoBehaviour, INameable, IPoolable, IInventoryHolder {
		
		public virtual string Name {
			get { return ""; }
		}

		public UnitTransform unitTransform;
		public UnitRenderer unitRenderer;
		public Inventory Inventory { get; protected set; }
		
		UnitInfoContent unitInfoContent = null;
		public UnitInfoContent UnitInfoContent { 
			get {
				if (unitInfoContent == null) {
					unitInfoContent = new UnitInfoContent (Name, Inventory);
				}
				return unitInfoContent;
			}
		}

		public Transform Transform {
			get { return unitTransform.MyTransform; }
		}

		public virtual void OnSelect () {
			unitRenderer.OnSelect ();
			unitTransform.OnSelect ();
			UnitInfoBox.Instance.Open (UnitInfoContent, Transform);
		}

		public virtual void OnUnselect () {
			unitRenderer.OnUnselect ();
			unitTransform.OnUnselect ();
			UnitInfoBox.Instance.Close ();
		}

		public virtual void OnCreate () {}
		public virtual void OnDestroy () {}
	}
}