using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using DNA.InventorySystem;

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

		ItemHolder holder;
		ItemHolder Holder {
			get {
				if (holder == null) {
					try {
						holder = Player.Instance.Inventory[groupId];
					} catch {
						throw new System.Exception ("Could not find an item group called '" + groupId + "'");
					}
				}
				return holder;
			}
		}

		void Start () {
			Holder.HolderUpdated += OnUpdate;
			OnUpdate ();
		}

		void OnUpdate () {
			CountText.text = Holder.Count.ToString ();
		}
	}
}