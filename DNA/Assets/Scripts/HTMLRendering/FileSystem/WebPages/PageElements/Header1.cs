using UnityEngine;
using System.Collections;

namespace FauxWeb {
	
	public class Header1 : TextElement {

		public Header1 (string text) {
			this.contents = new TextType[] {
				new Header (text)
			};
		}

		public Header1 (TextType[] contents) {
			this.contents = contents;
		}
	}
}