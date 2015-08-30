using UnityEngine;
using System.Collections;

namespace FauxWeb {

	public class Header : TextType {

		public Header (string text) {
			this.text = text;
			this.fontSize = 48;
			this.lineHeight = 38;
			this.fontColor = Color.black;
			this.style = FontStyle.Bold;
		}		
	}
}