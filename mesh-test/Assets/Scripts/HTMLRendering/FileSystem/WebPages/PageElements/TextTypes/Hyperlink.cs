using UnityEngine;
using System.Collections;

namespace FauxWeb {

	public class Hyperlink : TextType {

		bool visited = false;
		string destination;
		
		public Hyperlink (string text) {
			this.text = text;
			//this.fontSize = 14;
			//this.lineHeight = 16;
			this.fontColor = Color.blue;
			this.style = FontStyle.Normal;
		}		
	}
}