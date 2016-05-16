using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using InventorySystem;
using DNA.InputSystem;
using DNA.Units;

public class InfoBox : MBRefs {

	Canvas canvas = null;
	Canvas Canvas {
		get {
			if (canvas == null) {
				canvas = GetComponent<Canvas> ();
			}
			return canvas;
		}
	}

	public Text title;
	public Text[] inventoryRows;

	void Awake () {
		SelectionHandler.onUpdateSelection += OnUpdateSelection;
		Hide ();
	}

	void Update () {
		MyTransform.LookAt (Camera.main.transform);
	}

	void OnUpdateSelection (List<ISelectable> selected) {
		
		if (selected.Count != 1) {
			Hide ();
			return;
		}

		Show ();

		ISelectable selectable = selected[0];
		IInventoryHolder selInventory = selectable as IInventoryHolder;
		Unit unit = selectable as Unit;

		if (unit) title.text = unit.Name;

		foreach (Text row in inventoryRows)
			row.gameObject.SetActive (false);

		if (selInventory != null) {
			Dictionary<string, ItemGroup> groups = selInventory.Inventory.Groups.Where (x => x.Key != "Labor").ToDictionary (x => x.Key, x => x.Value);
			int counter = 0;
			foreach (var group in groups) {
				inventoryRows[counter].gameObject.SetActive (true);
				inventoryRows[counter].text = group.Value.Formatted (); 
				counter ++;
			}
		}
	}

	void Hide () {
		Canvas.enabled = false;
	}

	void Show () {
		Canvas.enabled = true;
	}
}
