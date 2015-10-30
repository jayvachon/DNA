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

	ItemGroup group;
	ItemGroup Group {
		get { return group; }
		set {
			if (group != null) {
				group.onUpdate -= SetText;
			}
			group = value;
			group.onUpdate += SetText;
			SetText ();
		}
	}

	public void Init (ItemGroup group) {
		Group = group;
		ItemName.text = Group.ID;
	}

	void SetText () {
		if (Group.HasCapacity) {
			Amount.text = Group.Count.ToString () + "/" + Group.Capacity.ToString ();
		} else {
			Amount.text = Group.Count.ToString ();
		}
	}
}
