using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GameInventory;

public class InventoryDrawer : MBRefs, IInventoryHolder {

	public Inventory Inventory { get; set; }

	public static void Create (Transform otherTransform, Inventory drawInventory) {
		GameObject go = new GameObject("InventoryDrawer", typeof (InventoryDrawer));
		go.transform.SetParent (otherTransform);
		go.transform.localPosition = Vector3.zero;
		InventoryDrawer drawer = go.GetComponent<InventoryDrawer>();
		drawer.Inventory = drawInventory;
	}

	void OnGUI () {
		Vector3 pos = Camera.main.WorldToScreenPoint(MyTransform.position);
		Vector2 labelPos = new Vector2 (pos.x, Screen.height - pos.y);
		GUI.color = Color.black;
		GUI.Label (new Rect (labelPos.x-50, labelPos.y, 200, 200), InventoryContents ());
	}

	string InventoryContents () {
		string contents = "";
		foreach (ItemHolder holder in Inventory.Holders) {
			int count = holder.Count;
			if (count > 0)
				contents += string.Format ("{0}: {1}/{2}\n", holder.Name, count, holder.Capacity);
		}
		return contents;
	}
}
