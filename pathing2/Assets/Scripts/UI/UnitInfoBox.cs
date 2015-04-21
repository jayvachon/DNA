using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using GameActions;
using GameInventory;

public class UnitInfoBox : MBRefs {

	public RectTransform contentGroup;

	BoxCollider contentCollider = null;
	BoxCollider ContentCollider {
		get {
			if (contentCollider == null) {
				contentCollider = contentGroup.GetComponent<BoxCollider> ();
			}
			return contentCollider;
		}
	}

	public Transform anchor;
	public Text title;
	public GameObject eldersText;
	public GameObject eldersGroup;
	public GameObject inventoryText;
	public GameObject inventoryGroup;
	public GameObject actionsText;
	public GameObject actionsGroup;

	UnitInfoContent content;
	Inventory inventory;
	PerformableActions performableActions;
	List<GameObject> elders = new List<GameObject> ();
	List<GameObject> holders = new List<GameObject> ();
	List<GameObject> actions = new List<GameObject> ();
	Vector3 creationPosition = Vector3.zero;

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
		creationPosition = LocalPosition;
		Close ();
	}

	void Update () {
		transform.localRotation = anchor.rotation;
		if (Input.GetKeyDown (KeyCode.Escape)) {
			Close ();
		}
	}

	/**
	 *	Opening
	 */

	public void Open (UnitInfoContent content, Transform transform) {
		
		this.content = content;
		content.contentUpdated += OnContentUpdated;
		title.text = content.Title;
		inventory = content.Inventory;
		inventory.inventoryUpdated += OnInventoryUpdated;
		performableActions = content.PerformableActions;
		if (performableActions != null) {
			performableActions.actionsUpdated += OnActionsUpdated;
		}
		MyTransform.SetParent (transform, false);
		LocalPosition = creationPosition;

		List<ItemHolder> itemHolders = inventory.Holders;
		InitInventory (itemHolders);
		InitElders (itemHolders);
		InitActions ();

		SetColliderSize ();
		Canvas.enabled = true;
	}

	void SetColliderSize () {
		// Not sure why this needs to wait a frame to work?
		StartCoroutine (CoSetColliderSize ());
	}

	IEnumerator CoSetColliderSize () {
		yield return new WaitForFixedUpdate ();
		float contentHeight = contentGroup.sizeDelta.y;
		ContentCollider.enabled = true;
		ContentCollider.SetCenterY (contentHeight/2);
		ContentCollider.SetSizeY (contentHeight);
	}

	void InitInventory (List<ItemHolder> itemHolders) {
		
		if (itemHolders.Count == 0)
			return;

		bool activateInventory = false;
		foreach (ItemHolder holder in itemHolders) {
			if (holder is ElderHolder) continue;
			if (holder.Count > 0 || holder.DisplaySettings.ShowWhenEmpty) {
				CreateHolder (holder);
				activateInventory = true;
			}
			/*if (!(holder is ElderHolder) && holder.Count > 0) {
				CreateHolder (holder);
				activateInventory = true;
			}*/
		}

		if (activateInventory)
			InventorySetActive (true);
	}

	void CreateHolder (ItemHolder holder) {
		Transform t = ObjectCreator.Instance.Create<ItemHolderContainerUI> ();
		t.SetParent (inventoryGroup.transform);
		t.Reset ();
		t.GetScript<ItemHolderContainerUI> ().Text = holder.DisplaySettings.ShowCapacity 
			? string.Format ("{0}: {1}/{2}", holder.Name, holder.Count, holder.Capacity)
			: string.Format ("{0}: {1}", holder.Name, holder.Count);
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
		for (int i = 0; i < elderItems.Count; i ++) {
			Transform t = ObjectCreator.Instance.Create<ElderAvatar> ();
			t.SetParent (eldersGroup.transform);
			t.localPosition = Vector3.zero;
			t.localEulerAngles = Vector3.zero;
			elders.Add (t.gameObject);
			t.GetScript<ElderAvatar> ().Init (elderItems[0]);
		}
	}

	void InitActions () {
		
		if (performableActions == null)
			return;

		foreach (var input in performableActions.Inputs) {
			CreateAction (input.Key, input.Value);
		}

		ActionsSetActive (performableActions.Inputs.Count > 0);
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
		
		if (inventory != null) {
			inventory.inventoryUpdated -= OnInventoryUpdated;
		}
		if (performableActions != null) {
			performableActions.actionsUpdated -= OnActionsUpdated;
		}
		if (content != null) {
			content.contentUpdated -= OnContentUpdated;
		}

		Canvas.enabled = false;
		ClearInventory ();
		ClearElders ();
		ClearActions ();
		DeactivateCollider ();
		MyTransform.SetParent (null);
	}

	void DeactivateCollider () {
		ContentCollider.enabled = false;
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
		ClearElders ();
		InitElders (inventory.Holders);
	}

	void OnActionsUpdated () {
		ClearActions ();
		InitActions ();
	}

	void OnActionButtonPress (string id) {
		performableActions.Start (id);
	}

	void OnContentUpdated () {
		ClearInventory ();
		InitInventory (inventory.Holders);
		ClearElders ();
		InitElders (inventory.Holders);
		ClearActions ();
		InitActions ();
		title.text = content.Title;
		SetColliderSize ();
	}
}