using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class InventoryHolderContainerUI : MBRefs, IPoolable {

	public Text text;
	public string Text {
		set { text.text = value; }
	}

	public void OnCreate () {}
	public void OnDestroy () {}
}
