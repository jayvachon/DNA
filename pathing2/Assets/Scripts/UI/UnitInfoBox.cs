using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using GameInventory;
using GameActions;

public class UnitInfoBox : MBRefs {

	public Text title;
	public GameObject contentGroup;
	public GameObject eldersText;
	public GameObject eldersGroup;
	public GameObject inventoryHolderText;
	public GameObject buttonContainer;
	List<GameObject> elderContainers = new List<GameObject> ();
	List<GameObject> inventoryContainers = new List<GameObject> ();
	List<GameObject> buttonContainers = new List<GameObject> ();

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
	PerformableActions performableActions;

	void Start () {
		Close ();
	}

	public void Open (UnitInfoContent content, Transform transform) {
		
		inventory = content.Inventory;
		inventory.inventoryUpdated += OnInventoryUpdated;
		title.text = content.Title;
		performableActions = content.PerformableActions;

		Canvas.enabled = true;
		MyTransform.SetParent (transform, false);
		InitInventory ();
		InitButtons ();
	}

	void OnInventoryUpdated () {
		ClearInventory ();
		InitInventory ();
	}

	public void Close () {
		Canvas.enabled = false;
		ClearInventory ();
		ClearButtons ();
		if (inventory != null) {
			inventory.inventoryUpdated -= OnInventoryUpdated;
		}
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

	void InitButtons () {
		if (performableActions == null) {
			return;
		}
		foreach (var input in performableActions.Inputs) {
			CreateButton (input.Key, input.Value);
		}
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

	void CreateButton (string id, string inputName) {
		Transform t = ObjectCreator.Instance.Create<ButtonContainer> ();
		t.SetParent (contentGroup.transform);
		t.localPosition = Vector3.zero;
		t.GetScript<ButtonContainer> ().Init (id, inputName, OnButtonPress);
		buttonContainers.Add (t.gameObject);
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

	void ClearButtons () {
		foreach (GameObject buttonContainer in buttonContainers) {
			ObjectPool.Destroy (buttonContainer);
		}
		buttonContainers.Clear ();
	}

	void OnButtonPress (string id) {
		performableActions.Start (id);
	}
}