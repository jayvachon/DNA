using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DNA.InputSystem;
using InventorySystem;

public class GuiInventory : GuiSelectableListener {

	void Awake () {
		Init ();
	}

	protected override void OnUpdateSelection (List<ISelectable> selected) {
		
		if (selected.Count == 0)
			return;
		
		List<Inventory> inventories = selected
			.FindAll (x => x is IInventoryHolder)
			.ConvertAll (x => ((IInventoryHolder)x).Inventory);
		
		Dictionary<string, ItemGroup> commonGroups = inventories[0].Groups;

		foreach (Inventory inventory in inventories) {
			Dictionary<string, ItemGroup> newGroups = inventory.Groups;
			foreach (var g in newGroups) {
				if (!commonGroups.ContainsKey (g.Key))
					commonGroups.Remove (g.Key);	
			}
			if (commonGroups.Count == 0)
				break;
		}

		foreach (var g in commonGroups) {
			// TODO: show groups in gui
		}
	}
}
