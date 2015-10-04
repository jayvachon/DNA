using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ItemHolderOverlay : MonoBehaviour {

	public Text itemHolder;

	public string Text {
		set { itemHolder.text = value; }
	}
}
