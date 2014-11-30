using UnityEngine;
using System.Collections;

public class Ul : PageElement {

	Li[] items;
	public Li[] Items {
		get { return items; }
	}

	public Ul (Li[] items) {
		this.items = items;
	}
}
