using UnityEngine;
using System.Collections;

namespace FauxWeb {

	// Styled text (such as Hyperlinks, Headers)
	public class TextType : PageElement {

		protected string text;
		public string Text {
			get { return text; }
			set { text = value; }
		}

		protected int fontSize = 16;
		public int FontSize {
			get { return fontSize; }
		}

		protected int lineHeight = 18;
		public int LineHeight {
			get { return lineHeight; }
		}

		protected FontStyle style;
		public FontStyle Style {
			get { return style; }
		}

		protected Color fontColor;
		public Color FontColor {
			get { return fontColor; }
		}
	}
}