using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DNA.InputSystem;
using InventorySystem;

public class GuiInventory : GuiSelectableListener {

	public List<GuiItemGroup> itemGroups;

	void Awake () {
		Init ();
	}

	protected override void OnUpdateSelection (List<ISelectable> selected) {
		
		if (selected.Count == 0) {
			SetGroupActive (false);
			return;
		}

		List<Inventory> inventories = selected
			.FindAll (x => x is IInventoryHolder)
			.ConvertAll (x => ((IInventoryHolder)x).Inventory);
		
		Dictionary<string, ItemGroup> commonGroups = inventories[0].Groups;
		bool hasGroups = false;

		foreach (Inventory inventory in inventories) {
			Dictionary<string, ItemGroup> newGroups = inventory.Groups;
			foreach (var g in newGroups) {
				if (!commonGroups.ContainsKey (g.Key))
					commonGroups.Remove (g.Key);	
			}
			if (commonGroups.Count == 0)
				break;
		}

		DisplableGroups ();
		foreach (var g in commonGroups) {
			EnableGroup (g.Value);
			hasGroups = true;
		}

		SetGroupActive (hasGroups);
	}

	void EnableGroup (ItemGroup g) {
		GuiItemGroup group = itemGroups.Find (x => !x.gameObject.activeSelf);
		if (group != null) {
			group.gameObject.SetActive (true);
			group.Init (g);
		}
	}

	void DisplableGroups () {
		foreach (GuiItemGroup group in itemGroups)
			group.gameObject.SetActive (false);
	}
}
