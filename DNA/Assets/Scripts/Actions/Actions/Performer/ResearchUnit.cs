using UnityEngine;
using System.Collections;
using Units;
using GameInventory;

namespace GameActions {

	public delegate void UnitUnlocked (string id);

	// T = Unit to be researched, U = ItemHolder to check
	public class ResearchUnit<T, U> : InventoryAction<U> where T : Unit where U : ItemHolder {

		string name = "";
		public override string Name {
			get { 
				if (name == "") {
					name = "Research" + TypeName;
				}
				return name;
			}
		}

		string TypeName {
			get { return typeof (T).Name; }
		}

		int cost = 0;
		int Cost {
			get { 
				#if VARIABLE_COST
				return CostValues.Instance.GetResearchCost (Name);
				#endif
				return cost; 
			}
			set { cost = value; }
		}

		UnitUnlocked unitUnlocked;

		public ResearchUnit (UnitUnlocked unitUnlocked) : base (-1, false, false) {
			Cost = CostValues.Instance.GetResearchCost (Name);
			this.unitUnlocked = unitUnlocked;
		}

		public override void Start () {
			if (Holder.Capacity < Cost) {
				Holder.Capacity = Cost;
			}
			if (Holder.Count >= Cost) {
				UnlockUnit ();
			} else {
				Holder.HolderUpdated += OnUpdated;
			}
		}

		public void OnUpdated () {
			if (Holder.Count >= Cost) {
				UnlockUnit ();
				Holder.HolderUpdated -= OnUpdated;
			}
		}

		public override void Stop () {
			base.Stop ();
			Holder.HolderUpdated -= OnUpdated;
		}

		void UnlockUnit () {
			StaticUnitsManager.UnlockUnit (TypeName);
			unitUnlocked (Name);
		}
	}
}