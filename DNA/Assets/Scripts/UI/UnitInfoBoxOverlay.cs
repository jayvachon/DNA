using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using GameInventory;
using GameActions;

public class UnitInfoBoxOverlay : MBRefs {

	static UnitInfoBoxOverlay instance = null;
	public static UnitInfoBoxOverlay Instance {
		get {
			if (instance == null) {
				instance = FindObjectOfType (typeof (UnitInfoBoxOverlay)) as UnitInfoBoxOverlay;
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

	public Text title;
	public Text description;
	public GameObject actionSection;
	public GameObject inventorySection;
	public GameObject actionsGroup;
	public GameObject inventoryGroup;
	public GameObject boxCollider;

	UnitInfoContent content;
	Inventory inventory;
	PerformableActions performableActions;

	List<GameObject> actions = new List<GameObject> ();
	List<GameObject> holders = new List<GameObject> ();

	void Start () {
		Close ();
	}

	public void Open (UnitInfoContent content) {
		this.content = content;
		content.contentUpdated += OnContentUpdated;
		OnContentUpdated ();
		boxCollider.SetActive (true);
	}

	public void Close () {
		
		if (inventory != null) 			inventory.inventoryUpdated -= OnInventoryUpdated;
		if (performableActions != null) performableActions.actionsUpdated -= OnActionsUpdated;
		if (content != null) 			content.contentUpdated -= OnContentUpdated;

		Canvas.enabled = false;
		boxCollider.SetActive (false);
	}

	void OnContentUpdated () {
		
		// Overview
		title.text = content.Title;
		description.text = content.Description;

		// Actions
		/*performableActions = content.PerformableActions;
		if (performableActions != null) {
			performableActions.actionsUpdated += OnActionsUpdated;
		}
		OnActionsUpdated ();*/

		// Inventory
		inventory = content.Inventory;
		inventory.inventoryUpdated += OnInventoryUpdated;
		OnInventoryUpdated ();

		Canvas.enabled = true;
	}

	void OnInventoryUpdated () {
		ClearInventory ();
		InitInventory ();
	}

	void OnActionsUpdated () {
		ClearActions ();
		InitActions ();
	}

	// Actions

	void InitActions () {
		
		if (performableActions == null) return;

		foreach (var action in performableActions.ActiveActions) {
			string name = action.Value.Name;
			if (performableActions.Inputs.ContainsKey (name)) {
				CreateAction (name, performableActions.Inputs[name]);
			}
		}

		actionSection.SetActive (actions.Count > 0);
	}

	void CreateAction (string id, string inputName) {
		Transform t = ObjectCreator.Instance.Create<ActionButtonOverlay> ();
		t.SetParent (actionsGroup.transform);
		t.Reset ();
		t.GetScript<ActionButtonOverlay> ().Init (id, inputName, OnActionButtonPress);
		actions.Add (t.gameObject);
	}

	void ClearActions () {
		foreach (GameObject action in actions) {
			ObjectPool.Destroy (action);
		}
		actions.Clear ();
	}

	void OnActionButtonPress (string id) {
		performableActions.Start (id);
	}

	// Inventory

	void InitInventory () {
		
		List<ItemHolder> itemHolders = inventory.Holders;
		if (itemHolders.Count == 0) return;

		foreach (ItemHolder holder in itemHolders) {
			if (holder.Count > 0 || holder.DisplaySettings.ShowWhenEmpty) {
				CreateHolder (holder);
			}
		}

		inventorySection.SetActive (holders.Count > 0);
	}

	void CreateHolder (ItemHolder holder) {
		Transform t = ObjectCreator.Instance.Create<ItemHolderOverlay> ();
		t.SetParent (inventoryGroup.transform);
		t.Reset ();
		t.GetScript<ItemHolderOverlay> ().Text = holder.DisplaySettings.ShowCapacity 
			? string.Format ("{0}: {1}/{2}", holder.Name, holder.Count, holder.Capacity)
			: string.Format ("{0}: {1}", holder.Name, holder.Count);
		holders.Add (t.gameObject);
	}

	void ClearInventory () {
		foreach (GameObject holder in holders) {
			ObjectPool.Destroy (holder);
		}
		holders.Clear ();
	}
}
