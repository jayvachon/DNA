using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using DNA.EventSystem;

public class Ground : MonoBehaviour, IPointerDownHandler {

	#region IPointerDownHandler implementation
	public void OnPointerDown (PointerEventData e) {
		Events.instance.Raise (new PointerDownEvent (this));
	}
	#endregion
}
