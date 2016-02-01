using UnityEngine;
using System.Collections;
using DNA.Tasks;
using InventorySystem;

namespace DNA.Units {

	// TODO: handle damaged apartments	

	public class Apartment : StaticUnit {

		protected override void OnEnable () {
			base.OnEnable ();
			Player.Instance.Inventory["Laborer"].Capacity += 9;
		}

		protected override void OnDisable () {
			base.OnDisable ();
			Player.Instance.Inventory["Laborer"].Capacity -= 9;
		}
	}
}