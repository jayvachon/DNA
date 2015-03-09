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

	ElderHolder elderHolder = null;
	UnitInfoContent content;
	Inventory inventory;

	public void Open (UnitInfoContent content, Transform transform) {
		
		this.content = content;
		inventory = content.Inventory;
		inventory.inventoryUpdated += OnInventoryUpdated;
		title.text = content.Title;

		Canvas.enabled = true;
		MyTransform.SetParent (transform, false);
		InitElders ();
		InitHolders ();
	}

	void OnInventoryUpdated () {
		ClearElders ();
		InitElders ();
		ClearHolders ();
		InitHolders ();
	}

	public void Close () {
		Canvas.enabled = false;
		ClearElders ();
		ClearHolders ();
		elderHolder = null;
		inventory.inventoryUpdated -= OnInventoryUpdated;
	}

	void InitElders () {
		elderHolder = inventory.Get<ElderHolder> () as ElderHolder;
		if (elderHolder != null && elderHolder.Count > 0) {
			eldersText.SetActive (true);
			eldersGroup.SetActive (true);
		} else {
			eldersText.SetActive (false);
			eldersGroup.SetActive (false);
		}
	}

	void ClearElders () {
		foreach (GameObject elderContainer in elderContainers) {
			ObjectPool.Destroy (elderContainer);
		}
		elderContainers.Clear ();
	}

	void InitHolders () {
		List<ItemHolder> holders = inventory.Holders;
		bool inventoryHasItems = false;
		foreach (ItemHolder holder in holders) {
			if (holder is ElderHolder) {
				CreateElders (holder as ElderHolder);
				continue;
			}
			if (holder.Count > 0) {
				Transform t = ObjectCreator.Instance.Create<InventoryHolderContainerUI> ();
				t.SetParent (contentGroup.transform);
				inventoryContainers.Add (t.gameObject);
				inventoryHasItems = true;
			}
		}
		if (inventoryHasItems) {
			inventoryHolderText.SetActive (true);
		} else {
			inventoryHolderText.SetActive (false);
		}
	}

	void ClearHolders () {
		foreach (GameObject container in inventoryContainers) {
			ObjectPool.Destroy (container);
		}
		inventoryContainers.Clear ();
	}

	void CreateElders (ElderHolder holder) {
		List<ElderItem> elders = holder.Items;
		foreach (ElderItem elder in elders) {
			Transform t = ObjectCreator.Instance.Create<ElderContainerUI> ();
			t.SetParent (eldersGroup.transform);
			elderContainers.Add (t.gameObject);
		}
	}
}