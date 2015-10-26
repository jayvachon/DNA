using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using InventorySystem;

namespace DNA {

	public class ItemGroupDrawer : MBRefs {

		public string groupId;

		Text countText = null;
		Text CountText {
			get {
				if (countText == null) {
					countText = MyTransform.GetChild (1).GetComponent<Text> ();
				}
				return countText;
			}
		}

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
			OnUpdate ();
		}

		void OnUpdate () {
			CountText.text = Group.Count.ToString ();
		}
	}
}