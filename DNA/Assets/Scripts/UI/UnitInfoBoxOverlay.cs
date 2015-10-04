using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using DNA.InventorySystem;
using DNA.Tasks;

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
	PerformableTasks performableTasks;

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
		
		if (inventory != null)	inventory.inventoryUpdated -= OnInventoryUpdated;
		if (content != null)	content.contentUpdated -= OnContentUpdated;

		Canvas.enabled = false;
		boxCollider.SetActive (false);
	}

	void OnContentUpdated () {
		
		// Overview
		title.text = content.Title;
		description.text = content.Description;

		// Tasks
		performableTasks = content.PerformableTasks;
		ClearTasks ();
		InitTasks ();

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

	// Tasks

	void InitTasks () {

		if (performableTasks != null) {
			foreach (var task in performableTasks.EnabledTasks) {
				Debug.Log (task.Value);
				if (task.Value.Settings.Title != "")
					CreateTask (task.Value);
			}
		}

		actionSection.SetActive (actions.Count > 0);
	}

	void CreateTask (PerformerTask task) {
		Transform t = ObjectPool.Instantiate<TaskButton> ().transform;
		t.SetParent (actionsGroup.transform);
		t.Reset ();
		t.GetScript<TaskButton> ().Init (task);
		actions.Add (t.gameObject);
	}

	void ClearTasks () {
		foreach (GameObject task in actions) {
			ObjectPool.Destroy (task);
		}
		actions.Clear ();
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
		Transform t = ObjectPool.Instantiate<ItemHolderOverlay> ().transform;
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
