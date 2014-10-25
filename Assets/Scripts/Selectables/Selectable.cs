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

	Hexagon onHexagon;
	public Hexagon OnHexagon {
		set { onHexagon = value; }
		get { return onHexagon; }
	}

	Color defaultColor = Color.white;
	Color selectColor = Color.grey;
	string[] keys = new string[0];
	string[] descriptions = new string[0];

	void Awake () {
		myTransform = transform;
		myTransform.tag = myTag;
		Events.instance.AddListener<KeyPressEvent>(OnKeyPressEvent);
		Events.instance.AddListener<MouseClickEvent>(OnMouseClickEvent);
	}

	void Start () {
		onHexagon = GM.ActiveStep.NearestHexagon (MyTransform.position);
		OnStart ();
	}

	public void Init (Color defaultColor, Color selectColor) {
		Init (defaultColor, selectColor, new string[0], new string[0]);
	}

	public void Init (Color defaultColor, Color selectColor, string[] keys, string[] descriptions) {
		this.defaultColor = defaultColor;
		this.selectColor = selectColor;
		this.keys = keys;
		this.descriptions = descriptions;
		renderer.SetColor (defaultColor);
	}

	public void ToggleSelect () {
		if (selected) Unselect ();
		else Select ();
	}
	
	public void Select () {
		if (!canSelect) return;
		if (selected) return;
		selected = true;
		renderer.SetColor (selectColor);
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
		renderer.SetColor (defaultColor);
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
