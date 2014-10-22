using UnityEngine;
using System.Collections;

public class Selectable : MonoBehaviour {

	string myTag = "Selectable";

	bool selected = false;
	public bool Selected {
		get { return selected; }
	}

	bool canSelect = true;
	public bool CanSelect {
		get { return canSelect; }
		set { canSelect = value; }
	}

	Transform myTransform;
	public Transform MyTransform {
		get { return myTransform; }
	}

	string[] keys = new string[0];
	string[] descriptions = new string[0];

	void Awake () {
		myTransform = transform;
		myTransform.tag = myTag;
	}

	void Start () {
		Events.instance.AddListener<KeyPressEvent>(OnKeyPressEvent);
		Events.instance.AddListener<MouseClickEvent>(OnMouseClickEvent);
		OnStart ();
	}

	public void Init (string[] keys, string[] descriptions) {
		this.keys = keys;
		this.descriptions = descriptions;
	}

	public void ToggleSelect () {
		if (selected) Unselect ();
		else Select ();
	}
	
	public void Select () {
		if (!canSelect) return;
		if (selected) return;
		selected = true;
		Events.instance.Raise (new SelectSelectableEvent (this));
		SetCommands ();
		OnSelect ();
	}

	private void SetCommands () {
		if (keys.Length > 0 && descriptions.Length > 0) {
			Events.instance.Raise (new SetCommandsEvent (keys, descriptions));
		}
	}

	public void Unselect () {
		if (!canSelect) return;
		if (!selected) return;
		selected = false;
		Events.instance.Raise (new UnselectSelectableEvent (this));
		ResetCommands ();
		OnUnselect ();
	}

	public void ResetCommands () {
		if (keys.Length > 0 && descriptions.Length > 0) {
			Events.instance.Raise (new ResetCommandsEvent ());
		}
	}

	private void OnKeyPressEvent (KeyPressEvent e) {
		if (!selected) return;
		for (int i = 0; i < keys.Length; i ++) {
			if (e.key == keys[i]) {
				OnKeyPress (e.key);
				return;
			}
		}
	}

	public virtual void OnMouseClickEvent (MouseClickEvent e) {
		if (e.transform.tag == myTag) {
			if (e.transform == MyTransform) ClickThis (e);
			else ClickOther (e);
		} else {
			ClickNothing (e);
		}
	}

	public virtual void ClickThis (MouseClickEvent e) {
		ToggleSelect ();
	}
	public virtual void ClickNothing (MouseClickEvent e) {
		Unselect ();
	}
	public virtual void ClickOther (MouseClickEvent e) {
		Unselect ();
	}

	public virtual void OnStart () {}
	public virtual void OnSelect () {}
	public virtual void OnUnselect () {}
	public virtual void OnKeyPress (string key) {}
}
