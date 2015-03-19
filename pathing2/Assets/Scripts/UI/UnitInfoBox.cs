using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using GameActions;
using GameInventory;

public class UnitInfoBox : MBRefs {

	public Transform anchor;
	public Text title;
	public GameObject eldersText;
	public GameObject eldersGroup;
	public GameObject inventoryText;
	public GameObject inventoryGroup;
	public GameObject actionsText;
	public GameObject actionsGroup;

	Inventory inventory;
	PerformableActions performableActions;
	List<GameObject> elders = new List<GameObject> ();
	List<GameObject> holders = new List<GameObject> ();
	List<GameObject> actions = new List<GameObject> ();

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

	void Start () {
		Close ();
	}

	void Update () {
		transform.localRotation = anchor.rotation;
	}

	/**
	 *	Opening
	 */

	public void Open (UnitInfoContent content, Transform transform) {
		
		inventory = content.Inventory;
		inventory.inventoryUpdated += OnInventoryUpdated;
		title.text = content.Title;
		performableActions = content.PerformableActions;
		MyTransform.SetParent (transform, false);

		List<ItemHolder> itemHolders = inventory.Holders;
		InitInventory (itemHolders);
		InitElders (itemHolders);
		InitActions ();
		Canvas.enabled = true;
	}

	void InitInventory (List<ItemHolder> itemHolders) {
		
		if (itemHolders.Count == 0)
			return;

		bool activateInventory = false;
		foreach (ItemHolder holder in itemHolders) {
			if (!(holder is ElderHolder) && holder.Count > 0) {
				CreateHolder (holder);
				activateInventory = true;
			}
		}

		if (activateInventory)
			InventorySetActive (true);
	}

	void CreateHolder (ItemHolder holder) {
		Transform t = ObjectCreator.Instance.Create<ItemHolderContainerUI> ();
		t.SetParent (inventoryGroup.transform);
		t.Reset ();
		t.GetScript<ItemHolderContainerUI> ().Text = string.Format ("{0}: {1}/{2}", holder.Name, holder.Count, holder.Capacity);
		holders.Add (t.gameObject);
	}

	void InitElders (List<ItemHolder> itemHolders) {

		if (itemHolders.Count == 0)
			return;

		foreach (ItemHolder holder in itemHolders) {
			if (holder is ElderHolder && holder.Count > 0) {
				CreateElders (holder as ElderHolder);
				EldersSetActive (true);
				break;
			}
		}
	}

	void CreateElders (ElderHolder holder) {
		List<ElderItem> elderItems = holder.Items;
		foreach (ElderItem elder in elderItems) {
			Transform t = ObjectCreator.Instance.Create<ElderAvatar> ();
			t.SetParent (eldersGroup.transform);
			t.localPosition = Vector3.zero;
			elders.Add (t.gameObject);
		}
	}

	void InitActions () {
		
		if (performableActions == null)
			return;

		foreach (var input in performableActions.Inputs) {
			CreateAction (input.Key, input.Value);
		}

		ActionsSetActive (true);
	}

	void CreateAction (string id, string inputName) {
		Transform t = ObjectCreator.Instance.Create<ActionButton> ();
		t.SetParent (actionsGroup.transform);
		t.Reset ();
		t.GetScript<ActionButton> ().Init (id, inputName, OnActionButtonPress);
		actions.Add (t.gameObject);
	}

	/**
	 *	Closing
	 */

	public void Close () {
		Canvas.enabled = false;
		ClearInventory ();
		ClearElders ();
		ClearActions ();
	}

	void ClearInventory () {
		foreach (GameObject holder in holders) {
			ObjectPool.Destroy (holder);
		}
		holders.Clear ();
		InventorySetActive (false);
	}

	void ClearElders () {
		foreach (GameObject elder in elders) {
			ObjectPool.Destroy (elder);
		}
		elders.Clear ();
		EldersSetActive (false);
	}

	void ClearActions () {
		foreach (GameObject action in actions) {
			ObjectPool.Destroy (action);
		}
		actions.Clear ();
		ActionsSetActive (false);
	}

	void InventorySetActive (bool active) {
		inventoryText.SetActive (active);
		inventoryGroup.SetActive (active);
	}

	void EldersSetActive (bool active) {
		eldersText.SetActive (active);
		eldersGroup.SetActive (active);
	}

	void ActionsSetActive (bool active) {
		actionsText.SetActive (active);
		actionsGroup.SetActive (active);
	}

	/**
	 *	Messages
	 */

	void OnInventoryUpdated () {
		ClearInventory ();
		InitInventory (inventory.Holders);
	}

	void OnActionButtonPress (string id) {
		performableActions.Start (id);
	}
}
