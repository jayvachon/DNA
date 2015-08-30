using UnityEngine;
using System.Collections;

namespace FauxWeb {

	public class UnorderedList : PageElement {

		ListItem[] items;
		public ListItem[] Items {
			get { return items; }
		}

		public UnorderedList (ListItem[] items) {
			this.items = items;
		}
	}
}