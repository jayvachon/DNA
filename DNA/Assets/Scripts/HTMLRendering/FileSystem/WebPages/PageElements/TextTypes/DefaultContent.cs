using UnityEngine;
using System.Collections;

namespace FauxWeb {

	public class DefaultContent : TextType {

		public DefaultContent (string text) {
			this.text = text;
			//this.fontSize = 14;
			//this.lineHeight = 16;
			this.fontColor = Color.black;
			this.style = FontStyle.Normal;
		}
	}
}