using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using GameInventory;

public class UnitInfoBox : MBRefs {

	public Text title;
	public GameObject contentGroup;
	public GameObject eldersText;
	public GameObject eldersGroup;
	public GameObject inventoryHolderText;
	List<GameObject> elderContainers = new List<GameObject> ();
	List<GameObject> inventoryContainers = new List<GameObject> ();

	static UnitInfoBox instance = null;
	public static UnitInfoBox Instance {
		get {
			if (instance == null) {
				instance = FindObjectOfType (typeof (UnitInfoBox)) as UnitInfoBox;
			}
			return instance;
		}
	}

	Canvas canvas = null;
	Canvas Canvas {
		get {
			if (canvas == null) {
				canvas = GetComponent<Canvas> ();
			}
			return canvas;
		}
	}

	Inventory inventory;

	public void Open (UnitInfoContent content, Transform transform) {
		
		inventory = content.Inventory;
		inventory.inventoryUpdated += OnInventoryUpdated;
		title.text = content.Title;

		Canvas.enabled = true;
		MyTransform.SetParent (transform, false);
		InitInventory ();
	}

	void OnInventoryUpdated () {
		ClearInventory ();
		InitInventory ();
	}

	public void Close () {
		Canvas.enabled = false;
		ClearInventory ();
		inventory.inventoryUpdated -= OnInventoryUpdated;
	}

	void InitInventory () {

		List<ItemHolder> holders = inventory.Holders;
		bool inventoryHasItems = false;
		bool inventoryHasElders = false;
		foreach (ItemHolder holder in holders) {
			if (holder is ElderHolder) {
				if (holder.Count > 0) {
					CreateElders (holder as ElderHolder);
					inventoryHasElders = true;
				}
				continue;
			}
			if (holder.Count > 0) {
				CreateHolder (holder);
				inventoryHasItems = true;
			}
		}

		if (inventoryHasItems) {
			inventoryHolderText.SetActive (true);
		} else {
			inventoryHolderText.SetActive (false);
		}

		if (inventoryHasElders) {
			eldersText.SetActive (true);
			eldersGroup.SetActive (true);
		} else {
			eldersText.SetActive (false);
			eldersGroup.SetActive (false);
		}
	}

	void ClearInventory () {
		
		foreach (GameObject container in inventoryContainers) {
			ObjectPool.Destroy (container);
		}
		inventoryContainers.Clear ();

		ClearElders ();
	}

	void ClearElders () {
		foreach (GameObject elderContainer in elderContainers) {
			ObjectPool.Destroy (elderContainer);
		}
		elderContainers.Clear ();
	}

	void CreateHolder (ItemHolder holder) {
		Transform t = ObjectCreator.Instance.Create<InventoryHolderContainerUI> ();
		t.SetParent (contentGroup.transform);
		t.localPosition = Vector3.zero;
		t.GetScript<InventoryHolderContainerUI> ().Text = string.Format ("{0}: {1}/{2}", holder.Name, holder.Count, holder.Capacity);
		inventoryContainers.Add (t.gameObject);
	}

	void CreateElders (ElderHolder holder) {
		List<ElderItem> elders = holder.Items;
		foreach (ElderItem elder in elders) {
			Transform t = ObjectCreator.Instance.Create<ElderContainerUI> ();
			t.SetParent (eldersGroup.transform);
			t.localPosition = Vector3.zero;
			elderContainers.Add (t.gameObject);
		}
	}
}