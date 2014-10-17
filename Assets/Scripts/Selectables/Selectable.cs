using UnityEngine;
using System.Collections;

public class Selectable : MonoBehaviour {

	string tag = "Selectable";

	bool selected = false;
	public bool Selected {
		get { return selected; }
	}

	Transform myTransform;
	public Transform MyTransform {
		get { return myTransform; }
	}

	void Awake () {
		myTransform = transform;
		myTransform.tag = tag;
	}

	void Start () {
		Events.instance.AddListener<MouseClickEvent>(OnMouseClickEvent);
		OnStart ();
	}

	public void ToggleSelect () {
		if (selected) Unselect ();
		else Select ();
	}
	
	public void Select () {
		if (selected) return;
		selected = true;
		Events.instance.Raise (new SelectSelectableEvent (this));
		OnSelect ();
	}
	
	public void Unselect () {
		if (!selected) return;
		selected = false;
		Events.instance.Raise (new UnselectSelectableEvent (this));
		OnUnselect ();
	}

	public virtual void OnMouseClickEvent (MouseClickEvent e) {
		if (e.transform.tag == tag) {
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
}
