using UnityEngine;
using System.Collections;

namespace GameInventory {

	public class ItemHolderDisplaySettings {
		
		public bool ShowWhenEmpty { get; private set; }
		public bool ShowCapacity { get; private set; }

		public ItemHolderDisplaySettings (bool showWhenEmpty=false, bool showCapacity=true) {
			ShowWhenEmpty = showWhenEmpty;
			ShowCapacity = showCapacity;
		}
	}
}