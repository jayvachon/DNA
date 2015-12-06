using UnityEngine;
using System.Collections;
using DNA.Tasks;
using InventorySystem;

namespace DNA.Units {

	public class Flower : StaticUnit {

		void Awake () {
			unitRenderer.SetColors (Palette.Yellow);
		}
	}
}