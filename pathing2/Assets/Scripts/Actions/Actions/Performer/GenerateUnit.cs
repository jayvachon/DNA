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
		UnitGenerated unitGenerated;

		public GenerateUnit (int cost, UnitGenerated unitGenerated=null) : base (-1, false, false) {
			this.cost = cost;
			this.unitGenerated = unitGenerated;
		}

		public override void Start () {
			if (Holder.Capacity < cost) {
				Holder.Capacity = cost;
			}
			if (Holder.Count >= cost) {
				CreateUnit ();
			} else {
				Holder.HolderUpdated += OnUpdated;
			}
		}

		public void OnUpdated () {
			if (Holder.Count >= cost) {
				CreateUnit ();
				Holder.HolderUpdated -= OnUpdated;
			}
		}

		void CreateUnit () {
			Holder.Remove (cost);
			Unit unit = ObjectCreator.Instance.Create<T> ().GetScript<Unit> ();
			if (unitGenerated != null) {
				unitGenerated (unit);
			}
		}
	}
}