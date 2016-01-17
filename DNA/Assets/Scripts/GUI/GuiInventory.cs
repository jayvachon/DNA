using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using DNA.InputSystem;
using InventorySystem;

namespace DNA {

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
				.Where (x => ((IInventoryHolder)x).Inventory != Player.Instance.Inventory)
				.ToList ()
				.ConvertAll (x => ((IInventoryHolder)x).Inventory);
			
			if (inventories.Count == 0) {
				SetGroupActive (false);
				return;
			}

			Dictionary<string, ItemGroup> commonGroups = inventories[0].Groups.Where (x => x.Key != "Labor").ToDictionary (x => x.Key, x => x.Value);;
			bool hasGroups = false;

			foreach (Inventory inventory in inventories) {
				Dictionary<string, ItemGroup> newGroups = inventory.Groups.Where (x => x.Key != "Labor").ToDictionary (x => x.Key, x => x.Value);
				foreach (var g in newGroups) {
					if (!commonGroups.ContainsKey (g.Key))
						commonGroups.Remove (g.Key);	
				}
				if (commonGroups.Count == 0)
					break;
			}

			DisableGroups ();
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

		void DisableGroups () {
			foreach (GuiItemGroup group in itemGroups)
				group.gameObject.SetActive (false);
		}
	}
}