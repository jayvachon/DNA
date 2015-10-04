using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ItemHolderContainerUI : MonoBehaviour {

	public Text itemHolder;

	public string Text {
		set { itemHolder.text = value; }
	}
}
