using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DNA.InventorySystem;

public class InventoryDrawer : MBRefs, IInventoryHolder {

	public Inventory Inventory { get; set; }

	public static InventoryDrawer Create (Transform otherTransform, Inventory drawInventory) {
		GameObject go = new GameObject("InventoryDrawer", typeof (InventoryDrawer));
		go.transform.SetParent (otherTransform);
		go.transform.localPosition = Vector3.zero;
		InventoryDrawer drawer = go.GetComponent<InventoryDrawer>();
		drawer.Inventory = drawInventory;
		return drawer;
	}

	void OnGUI () {
		Vector2 labelPos = V2Position;
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

	/**
	 *	Debugging
	 */

	public void Print () {
		foreach (ItemHolder holder in Inventory.Holders) {
			Debug.Log(holder.Count);
			Debug.Log(holder.Name);
		}
	}
}
