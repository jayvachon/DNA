using UnityEngine;
using System.Collections;

namespace FauxWeb {

	// TextElements (such as Header1 and Paragraph) contain TextTypes (such as Hyperlinks)
	public class TextElement : PageElement {

		protected TextType[] contents;
		public TextType[] Contents {
			get { return contents; }
		}
	}
}