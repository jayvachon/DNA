using UnityEngine;
using System.Collections;

namespace FauxWeb {
	
	public class ListItem : PageElement {

		PageElement element;
		public PageElement Element {
			get { return element; }
		}

		public ListItem (PageElement element) {
			this.element = element;
		}
	}
}