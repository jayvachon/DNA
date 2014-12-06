using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace FauxWeb {
	
	public class Paragraph : TextElement {

		public Paragraph (string text) {
			this.contents = new TextType[] {
				new DefaultContent (text)
			};
		}

		public Paragraph (TextType[] contents) {
			this.contents = contents;
		}
	}
}