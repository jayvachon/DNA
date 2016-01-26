using UnityEngine;
using System.Collections;
using DNA.Units;

namespace DNA.Tasks {

	public class GenerateLaborer : GenerateUnit<Laborer> {

		public override bool Enabled {
			get { 
				Debug.Log (Player.Instance.Inventory["Laborer"].Count);
				Debug.Log (Player.Instance.Inventory["Laborer"].Capacity);
				Debug.Log (Player.Instance.Inventory["Laborer"].Full);
				return CanAfford && !Player.Instance.Inventory["Laborer"].Full; }
		}
	}
}