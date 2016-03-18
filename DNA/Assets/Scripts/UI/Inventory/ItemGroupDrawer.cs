using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using InventorySystem;

namespace DNA {

	public class ItemGroupDrawer : MBRefs {

		public string groupId;
		public Text countText;

		ItemGroup group;
		ItemGroup Group {
			get {
				if (group == null) {
					try {
						group = Player.Instance.Inventory[groupId];
					} catch {
						throw new System.Exception ("Could not find an item group called '" + groupId + "'");
					}
				}
				return group;
			}
		}

		void Start () {
			Group.onUpdate += OnUpdate;
			Group.onUpdateCapacity += OnUpdate;
			OnUpdate ();
		}

		void OnUpdate () {
			if (Group.ID == "Laborer") {
				countText.text = Group.Count.ToString () + "/" + Group.Capacity.ToString ();
			} else {
				string suffix = "";
				/*string suffix = "M";
				if (Group.ID == "Coffee")
					suffix = "C";*/
				countText.text = Group.Count.ToString () + suffix;
			}
		}
	}
}