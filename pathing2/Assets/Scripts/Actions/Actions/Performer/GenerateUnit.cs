using UnityEngine;
using System.Collections;
using Units;
using GameInventory;

namespace GameActions {

	public delegate void UnitGenerated (Unit unit);

	// T = Unit to be generated, U = ItemHolder to check
	public class GenerateUnit<T, U> : InventoryAction<U> where T : Unit where U : ItemHolder {

		string name = "";
		public override string Name {
			get { 
				if (name == "") {
					string typeName = typeof (T).Name;
					name = "Generate" + typeName;
				}
				return name;
			}
		}

		int cost = 0;
		int Cost {
			get { 
				#if VARIABLE_COST
				return CostValues.Instance.GetCost (Name);
				#endif
				return cost; 
			}
			set { cost = value; }
		}

		UnitGenerated unitGenerated;

		public GenerateUnit (int cost=-1, UnitGenerated unitGenerated=null) : base (-1, false, false) {
			this.cost = (cost == -1) ? CostValues.Instance.GetCost (Name) : cost;
			this.unitGenerated = unitGenerated;
		}

		public override void Start () {
			if (Holder.Capacity < Cost) {
				Holder.Capacity = Cost;
			}
			if (Holder.Count >= Cost) {
				CreateUnit ();
			} else {
				Holder.HolderUpdated += OnUpdated;
			}
		}

		public void OnUpdated () {
			if (Holder.Count >= Cost) {
				CreateUnit ();
				Holder.HolderUpdated -= OnUpdated;
			}
		}

		void CreateUnit () {
			Holder.Remove (Cost);
			Unit unit = ObjectCreator.Instance.Create<T> ().GetScript<Unit> ();
			if (unitGenerated != null) {
				unitGenerated (unit);
			}
		}
	}
}