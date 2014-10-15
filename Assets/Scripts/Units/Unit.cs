using UnityEngine;
using System.Collections;

public class Unit : MonoBehaviour {

	bool selected = false;
	Transform myTransform;

	Color defaultColor = Color.red;
	Color selectedColor = Color.magenta;

	Vector3 destPosition;

	void Awake () {
		myTransform = transform;
	}

	void Start () {
		Events.instance.AddListener<MouseClickEvent>(OnMouseClickEvent);
		renderer.SetColor (defaultColor);
	}

	void OnMouseClickEvent (MouseClickEvent e) {
		if (e.transform == myTransform) {
			ToggleSelect ();
		} else {
			if (selected) {
				StartMove (e.point);
			}
		}
	}

	void ToggleSelect () {
		if (selected) Unselect ();
		else Select ();
	}
	
	void Select () {
		selected = true;
		renderer.SetColor (selectedColor);
		Events.instance.Raise (new SelectUnitEvent (this));
	}

	void Unselect () {
		selected = false;
		renderer.SetColor (defaultColor);
		Events.instance.Raise (new UnselectUnitEvent (this));
	}

	void StartMove (Vector3 destination) {
		destPosition = destination;
		StartCoroutine (Move ());
	}

	IEnumerator Move () {
		float speed = 0.5f;
		while (Vector3.Distance (rigidbody.position, destPosition) > 1) {
			rigidbody.MovePosition (Vector3.Lerp (rigidbody.position, destPosition, Time.deltaTime * speed));
			yield return null;
		}
	}
}
