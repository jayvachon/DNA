using UnityEngine;
using System.Collections;
using Units;
using GameInventory;

namespace GameActions {

	public delegate void UnitGenerated (Unit unit);

	// T = Unit to be generated, U = ItemHolder to check
	public class GenerateUnit<T, U> : PerformerAction where T : Unit where U : ItemHolder {

		int cost = 0;
		Vector3 createPosition;
		UnitGenerated unitGenerated;

		U holder = null;
		U Holder {
			get {
				if (holder == null) {
					holder = Inventory.Get<U> () as U;
				}
				if (holder == null) {
					Debug.LogError ("Inventory does not include " + typeof (U));
				}
				return holder;
			}
		}

		public GenerateUnit (int cost, Vector3 createPosition, UnitGenerated unitGenerated=null) : base (0, false, false) {
			this.cost = cost;
			this.createPosition = createPosition;
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
			unit.Position = createPosition;
			if (unitGenerated != null) {
				unitGenerated (unit);
			}
		}
	}
}