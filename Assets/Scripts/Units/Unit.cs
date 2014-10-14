using UnityEngine;
using System.Collections;

public class Unit : MonoBehaviour {

	private bool selected = false;
	private Transform myTransform;

	void Awake () {
		myTransform = transform;
	}

	void Start () {
		Events.instance.AddListener<MouseClickEvent>(OnMouseClickEvent);
		renderer.SetColor (Color.red);
	}

	void OnMouseClickEvent (MouseClickEvent e) {
		if (e.transform != myTransform) return;
		selected = true;
	}
}
