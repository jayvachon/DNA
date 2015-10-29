using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using InventorySystem;

public class GuiItemGroup : MBRefs {

	Text itemName = null;
	Text ItemName {
		get {
			if (itemName == null) {
				itemName = MyTransform.GetChild (0).GetComponent<Text> ();
			}
			return itemName;
		}
	}

	Text amount = null;
	Text Amount {
		get {
			if (amount == null) {
				amount = MyTransform.GetChild (1).GetComponent<Text> ();
			}
			return amount;
		}
	}

	public void Init (ItemGroup group) {
		ItemName.text = group.ID;
		if (group.HasCapacity) {
			Amount.text = group.Count.ToString () + "/" + group.Capacity.ToString ();
		} else {
			Amount.text = group.Count.ToString ();
		}
	}
}
