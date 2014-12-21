using UnityEngine;
using System.Collections;

public interface ISelectable {

	bool Selected { get; }
	bool Selectable { get; set; }

	void Select ();
	void Unselect ();
	void ToggleSelect ();
}
